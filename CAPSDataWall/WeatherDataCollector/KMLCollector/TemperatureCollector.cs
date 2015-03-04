using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeatherAPIModels.Constants;
using WeatherAPIModels.Models;
using WeatherDataCollector.KMLFormats;
using WeatherDataCollector.Requests;
using WeatherDataCollector.StorageProvider;

namespace WeatherDataCollector.KMLCollector
{
    public class TemperatureCollector : ICollector
    {
        private IStorageProvider StorageProvider { get; set; }
        private KMLStreamDescription StreamDescription { get; set; }
        private Timer Collector { get; set; }
        private WeatherDataAPIClient Client { get; set; }

        public TemperatureCollector(IPermanentStorageProvider storageProvider, KMLStreamDescription streamDescription)
        {
            this.StorageProvider = storageProvider;
            this.StreamDescription = streamDescription;

            this.Client = new WeatherDataAPIClient();
            this.Collector = null;
        }
        public void StartCollector()
        {
            //Timer already started
            if (this.Collector != null)
            {
                return;
            }

            this.Collector = new Timer(async (e) =>
            {
                var currentTime = DateTime.Now;

                if (currentTime.Minute % 10 == 0)
                {
                    return;
                }

                var sanatizedCurrentTime = currentTime - new TimeSpan(0, 0, currentTime.Second);

                var tempFileName = Path.GetTempFileName();

                var temperatureDataResponse = NOAARequests.GetTemperatureData(StorageProvider,tempFileName);

                if (!temperatureDataResponse)
                {
                    Console.WriteLine("Could not get Radar Data in Temperature Collector!");
                    return;
                }

                var addParams = new StorageProviderAddParams()
                {
                    ServerFolderName = this.StreamDescription.Type.Name,
                    ServerFileName = sanatizedCurrentTime + ".kmz",
                    LocalFileName = tempFileName,
                    ContentType = this.StreamDescription.Type.FileType.ContentType
                };

                var storageUrl = StorageProvider.Add(addParams);

                File.Delete(tempFileName);

                //Set useable url to null if storage provider doesn't support it
                string useableUrl = null;

                if (!this.StreamDescription.Type.FileType.RequiresKMLFileCreation
                    || StorageProvider is IKMLUseableStorageProvider)
                {
                    useableUrl = storageUrl;
                }

                var kmlData = new KMLData
                {
                    CreatedAt = sanatizedCurrentTime,
                    FileName = sanatizedCurrentTime + "." + this.StreamDescription.Type.FileType.Name,
                    StorageUrl = storageUrl,
                    UseableUrl = useableUrl,
                    DataType = this.StreamDescription.Type
                };

                var response = await this.Client.AddKMLData(kmlData);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Could not update kml stream!");
                    return;
                }

                Console.WriteLine("Temperature data uploaded to API!");
                kmlData = await response.Content.ReadAsAsync<KMLData>();

                response  = await Client.UpdateKMLStream(this.StreamDescription,kmlData.ID);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Could not update stream status!");
                }
            }, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

        }

        public void StopCollector()
        {
            if (this.Collector != null)
            {
                this.Collector.Dispose();
            }

        }

    }
}
