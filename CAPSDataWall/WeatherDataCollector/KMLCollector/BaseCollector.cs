using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeatherAPIModels.Models;
using WeatherAPIModels.StreamDescriptions;
using WeatherDataCollector.Requests;
using WeatherDataCollector.StorageProvider;

namespace WeatherDataCollector.KMLCollector
{
    public class BaseCollector : ICollector
    {
        protected IStorageProvider StorageProvider { get; set; }
        protected KMLStreamDescription StreamDescription { get; set; }
        protected Timer Collector { get; set; }
        protected WeatherDataAPIClient Client { get; set; }
        protected TimeSpan CheckFrequency { get; set; }

        protected Func<DateTime,bool> ShouldRunUpdate {get; set; }
        protected Func<IStorageProvider,string,bool> RequestData {get; set; }

        public BaseCollector(IPermanentStorageProvider storageProvider, KMLStreamDescription streamDescription,TimeSpan checkFrequency, Func<DateTime,bool> shouldRunUpdate, Func<IStorageProvider,string,bool> requestData)
        {
            this.StorageProvider = storageProvider;
            this.StreamDescription = streamDescription;
            this.CheckFrequency = checkFrequency; 

            this.ShouldRunUpdate = shouldRunUpdate;
            this.RequestData = requestData;

            this.Client = new WeatherDataAPIClient();
            this.Collector = null;
        }
        public virtual void StartCollector()
        {
            //Timer already started
            if (this.Collector != null)
            {
                return;
            }

            this.Collector = new Timer(async e =>
            {
                var currentTime = DateTime.Now;

                if (!this.ShouldRunUpdate(currentTime))
                {
                    return;
                }

                var sanatizedCurrentTime = currentTime - new TimeSpan(0, 0, currentTime.Second);

                var tempFileName = Path.GetTempFileName();

                var dataRequestResponse = RequestData(StorageProvider,tempFileName);

                if (!dataRequestResponse)
                {
                    Console.WriteLine("Could not download {0} data!", this.StreamDescription.KMLDataType.Name);
                    return;
                }

                var addParams = new StorageProviderAddParams()
                {
                    ServerFolderName = this.StreamDescription.KMLDataType.Name,
                    ServerFileName = sanatizedCurrentTime + "." + this.StreamDescription.KMLDataType.FileType.Name,
                    LocalFileName = tempFileName,
                    ContentType = this.StreamDescription.KMLDataType.FileType.ContentType
                };

                var storageUrl = StorageProvider.Add(addParams);

                File.Delete(tempFileName);

                //Set useable url to null if storage provider doesn't support it
                string useableUrl = null;

                if (!this.StreamDescription.KMLDataType.FileType.RequiresKMLFileCreation
                    || StorageProvider is IKMLUseableStorageProvider)
                {
                    useableUrl = storageUrl;
                }

                var kmlData = new KMLData
                {
                    CreatedAt = sanatizedCurrentTime,
                    FileName = sanatizedCurrentTime + "." + this.StreamDescription.KMLDataType.FileType.Name,
                    StorageUrl = storageUrl,
                    UseableUrl = useableUrl,
                    DataType = this.StreamDescription.KMLDataType
                };

                var response = await this.Client.AddKMLData(kmlData);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Could not update {0} kml stream!", this.StreamDescription.KMLDataType.Name);
                    return;
                }

                Console.WriteLine("{0} data uploaded to API!", this.StreamDescription.KMLDataType.Name);
                kmlData = await response.Content.ReadAsAsync<KMLData>();

                response  = await Client.UpdateKMLStream(this.StreamDescription,kmlData.ID);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Could not update {0} kml stream", this.StreamDescription.KMLDataType.Name);
                }

            }, null, TimeSpan.Zero,this.CheckFrequency);

        }

        public virtual void StopCollector()
        {
            if (this.Collector != null)
            {
                this.Collector.Dispose();
            }

        }

    }
}
