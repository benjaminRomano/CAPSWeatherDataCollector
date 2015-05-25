using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WeatherAPIClients.Clients;
using WeatherAPIModels.KMLFormatters;
using WeatherAPIModels.Models;
using WeatherAPIModels.Utilities;
using WeatherDataCollector.Constants;
using WeatherDataCollector.Requests;
using WeatherDataCollector.StorageProvider;

namespace WeatherDataCollector.StreamGetters
{
    class StreamGetter : IStreamGetter
    {
        private string FilePath { get; set; }
        private Timer StreamGetterTimer { get; set; }
        private WeatherAPIClient Client { get; set; }
        private StreamDescription StreamDescription { get; set; }
        private IKMLUseableStorageProvider StorageProvider { get; set; }
        private TimeSpan UpdateFrequency { get; set; }

        public StreamGetter(IKMLUseableStorageProvider storageProvider, StreamDescription streamDescription, string filePath, TimeSpan updateFrequency)
        {
            this.StorageProvider = storageProvider;
            this.StreamDescription = streamDescription;
            this.FilePath = filePath;
            this.UpdateFrequency = updateFrequency;

            this.Client = new WeatherAPIClient(WeatherDataConstants.WeatherAPIUri);

            this.StreamGetterTimer = null;
        }

        public void Start()
        {
            if (this.StreamGetterTimer != null)
            {
                return;
            }

            this.StreamGetterTimer = new Timer(async e =>
            {
                var response = await Client.KMLStream.GetKMLStream(this.StreamDescription);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Failed to get stream");
                    return;
                }

                var stream = await response.Content.ReadAsAsync<KMLStream>();
                    
                //Upload kml data to kml useable storage if not already in kml useable storage
                if (!IsValidUseableUrl(stream.KMLData.UseableUrl) && stream.KMLData.DataType.FileType.RequiresKMLFileCreation)
                {
                    var useableUrl = await UploadStreamKMLDataToUseableStorage(stream);

                    if (useableUrl == null)
                    {
                        Console.WriteLine("Could not upload kml data to useable storage");
                        return;
                    }

                    var success = await UpdateKMLDataUseableUrl(stream.KMLData, useableUrl);

                    if (!success)
                    {
                        Console.WriteLine("Could not update kml data useable url");
                        return;
                    }
                }

                var updated = UpdateLocalStreamFile(stream);

                if (!updated)
                {
                    Console.WriteLine("Failed to update local stream file");
                    return;
                }

            }, null, TimeSpan.Zero, this.UpdateFrequency);
        }

        private bool IsValidUseableUrl(string useableUrl)
        {
            if (useableUrl == null)
            {
                return false;
            }

            var tempFileName = Path.GetTempFileName();
            var success = RequestHelper.DownloadFile(useableUrl, tempFileName);

            File.Delete(tempFileName);

            return success;
        }

        private bool UpdateLocalStreamFile(KMLStream stream)
        {
            //Delete existing file so it can be re-created
            try
            {
                File.Delete(this.FilePath);
            }
            catch (Exception)
            {
                return false;
            }

            if (stream.KMLData.DataType.FileType.RequiresKMLFileCreation)
            {
                var kmlFileCreator = new KMLFileCreator();

                kmlFileCreator.CreateKMLFile(stream.KMLData.DataType, stream.KMLData.UseableUrl, this.FilePath);
            }
            else
            {
                var success = RequestHelper.DownloadFile(stream.KMLData.UseableUrl, this.FilePath);

                if (!success)
                {
                    Console.WriteLine("Could not download file for {0} stream", this.StreamDescription);
                    return false;
                }
            }

            Console.WriteLine("{0} stream updated!", this.StreamDescription);

            return true;
        }

        private async Task<bool> UpdateKMLDataUseableUrl(KMLData kmlData, string useableUrl)
        {
            kmlData.UseableUrl = useableUrl;

            var response = await this.Client.KMLData.PutKMLData(kmlData);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Could not Update KML Data with useable URL");
                return false;
            }

            return true;
        }

        private async Task<string> UploadStreamKMLDataToUseableStorage(KMLStream stream)
        {
            var tempFileName = Path.GetTempFileName();

            var downloadSuccessful = RequestHelper.DownloadFile(stream.KMLData.StorageUrl, tempFileName);

            if (!downloadSuccessful)
            {
                return null;
            }

            var addParams = new StorageProviderAddParams()
            {
                ServerFolderName = stream.KMLData.DataType.Name,
                ServerFileName = stream.KMLData.FileName,
                LocalFileName = tempFileName,
                ContentType = stream.KMLData.DataType.FileType.ContentType
            };

            var useableUrl = this.StorageProvider.Add(addParams);

            return useableUrl;
        }

        public void Stop()
        {
            if (this.StreamGetterTimer != null)
            {
                this.StreamGetterTimer.Dispose();
            }

            this.StreamGetterTimer = null;
        }
    }
}