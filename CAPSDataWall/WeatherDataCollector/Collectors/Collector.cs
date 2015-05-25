using System;
using System.IO;
using System.Threading;
using WeatherAPIClients.Clients;
using WeatherAPIModels.Models;
using WeatherDataCollector.Constants;
using WeatherDataCollector.StorageProvider;

namespace WeatherDataCollector.Collectors
{
    public class Collector : ICollector
    {
        protected IStorageProvider StorageProvider { get; set; }
        protected KMLDataType KMLDataType { get; set; }
        protected Timer CollectorTimer { get; set; }
        protected WeatherAPIClient Client { get; set; }
        protected TimeSpan CheckFrequency { get; set; }

        protected Func<DateTime,bool> ShouldRunUpdate {get; set; }
        protected Func<IStorageProvider,string,bool> RequestData {get; set; }

        public Collector(IPermanentStorageProvider storageProvider, KMLDataType kmlDataType,TimeSpan checkFrequency, Func<DateTime,bool> shouldRunUpdate, Func<IStorageProvider,string,bool> requestData)
        {
            this.StorageProvider = storageProvider;
            this.KMLDataType = kmlDataType;
            this.CheckFrequency = checkFrequency; 

            this.ShouldRunUpdate = shouldRunUpdate;
            this.RequestData = requestData;

            this.Client = new WeatherAPIClient(WeatherDataConstants.WeatherAPIUri);
            this.CollectorTimer = null;
        }

        public void StartCollector()
        {
            if (this.CollectorTimer != null)
            {
                return;
            }

            this.CollectorTimer = new Timer(async e =>
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
                    Console.WriteLine("Could not download {0} data!", this.KMLDataType.Name);
                    return;
                }

                var addParams = new StorageProviderAddParams()
                {
                    ServerFolderName = this.KMLDataType.Name,
                    ServerFileName = sanatizedCurrentTime + "." + this.KMLDataType.FileType.Name,
                    LocalFileName = tempFileName,
                    ContentType = this.KMLDataType.FileType.ContentType
                };

                var storageUrl = StorageProvider.Add(addParams);

                File.Delete(tempFileName);

                //Set useable url to null if storage provider doesn't support it
                string useableUrl = null;

                if (!this.KMLDataType.FileType.RequiresKMLFileCreation
                    || StorageProvider is IKMLUseableStorageProvider)
                {
                    useableUrl = storageUrl;
                }

                var kmlData = new KMLData
                {
                    CreatedAt = sanatizedCurrentTime,
                    FileName = sanatizedCurrentTime + "." + this.KMLDataType.FileType.Name,
                    StorageUrl = storageUrl,
                    UseableUrl = useableUrl,
                    DataType = this.KMLDataType
                };

                var response = await this.Client.KMLData.PutKMLData(kmlData);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Could not add {0} data!", this.KMLDataType.Name);
                    return;
                }

                Console.WriteLine("{0} data uploaded to API!", this.KMLDataType.Name);

            }, null, TimeSpan.Zero,this.CheckFrequency);

        }

        public void StopCollector()
        {
            if (this.CollectorTimer != null)
            {
                this.CollectorTimer.Dispose();
            }
        }
    }
}
