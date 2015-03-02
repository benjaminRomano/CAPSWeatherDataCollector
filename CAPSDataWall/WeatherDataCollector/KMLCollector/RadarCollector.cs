using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WeatherAPIModels;
using WeatherDataCollector.Constants;
using WeatherDataCollector.KMLFormats;
using WeatherDataCollector.Requests;
using WeatherDataCollector.StorageProvider;

namespace WeatherDataCollector.KMLCollector
{
    class RadarCollector : ICollector
    {

        private IStorageProvider StorageProvider { get; set; }
        private KMLStreamDescription StreamDescription { get; set; }
        private Timer Collector { get; set; }
        private WeatherDataAPIClient Client { get; set; }

        public RadarCollector(IPermanentStorageProvider storageProvider, KMLStreamDescription streamDescription)
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

                if (currentTime.Minute % 10 != 0)
                {
                    return;
                }

                var sanatizedCurrentTime = currentTime - new TimeSpan(0, 0, currentTime.Second);

                var tempFileName = Path.GetTempFileName();

                var radarDataResponse = NOAARequests.RadarDataRequest(StorageProvider,tempFileName);
                if (!radarDataResponse)
                {
                    Console.WriteLine("Could not get Radar Data in Radar Collector!");
                    return;
                }

                var addParams = new StorageProviderAddParams()
                {
                    ServerFolderName = Enum.GetName(typeof (KMLDataType), KMLDataType.Radar),
                    ServerFileName = sanatizedCurrentTime + ".gif",
                    LocalFileName = tempFileName,
                    ContentType = WeatherDataConstants.ContentTypesForData[KMLDataType.Radar]
                };

                var storageUrl = StorageProvider.Add(addParams);

                File.Delete(tempFileName);


                //Set useable url to null if storage provider doesn't support it
                string useableUrl = null;
                if (StorageProvider is IKMLUseableStorageProvider)
                {
                    useableUrl = storageUrl;
                }

                var kmlData = new KMLData
                {
                    CreatedAt = sanatizedCurrentTime,
                    FileName = sanatizedCurrentTime + ".gif",
                    StorageUrl = storageUrl,
                    UseableUrl = useableUrl,
                    Type = KMLDataType.Radar
                };

                var response = await this.AddRadarData(kmlData);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Could not update stream status!");
                    return;
                }

                Console.WriteLine("Radar data uploaded to API!");
                kmlData = await response.Content.ReadAsAsync<KMLData>();

                response  = await Client.UpdateStreamStatus(this.StreamDescription,kmlData.ID);

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

        public Task<HttpResponseMessage> AddRadarData(KMLData kmlData)
        {
            return Client.AddKMLData(kmlData);
        }
    }
}
