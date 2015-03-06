using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WeatherAPIModels.KMLFormatters;
using WeatherAPIModels.Models;
using WeatherAPIModels.StreamDescriptions;
using WeatherDataCollector.Requests;
using WeatherDataCollector.StorageProvider;

namespace WeatherDataCollector.Streams
{
    public abstract class BaseStream : IStream
    {
        public abstract void StartStream();

        protected string FilePath { get; set; }
        protected Timer Stream { get; set; }
        protected WeatherDataAPIClient Client { get; set; }
        protected KMLStreamDescription InputStreamDescription { get; set; }
        protected KMLStreamDescription OutputStreamDescription { get; set; }
        protected IKMLUseableStorageProvider StorageProvider { get; set; }
        protected TimeSpan UpdateFrequency { get; set; }

        protected BaseStream(IKMLUseableStorageProvider storageProvider,KMLStreamDescription inputStreamDescription,KMLStreamDescription outputStreamDescription, string filePath, TimeSpan updateFrequency)
        {
            this.StorageProvider = storageProvider;
            this.InputStreamDescription = inputStreamDescription;
            this.OutputStreamDescription = outputStreamDescription;
            this.FilePath = filePath;
            this.UpdateFrequency = updateFrequency;

            this.Client = new WeatherDataAPIClient();

            this.Stream = null;
        }

        public virtual void StopStream()
        {
            if (Stream != null)
            {
                this.Stream.Dispose();
            }
            this.Stream = null;
        }

        protected virtual async Task<bool> UpdateLocalStreamFile(KMLStream stream)
        {
            var tempFileName = Path.GetTempFileName();

            var downloadSuccessful = RequestHelper.DownloadFile(stream.KMLData.StorageUrl, tempFileName);

            if (!downloadSuccessful)
            {
                return false;
            }

            if (stream.KMLData.UseableUrl == null && stream.KMLData.DataType.FileType.RequiresKMLFileCreation)
            {
                var addParams = new StorageProviderAddParams()
                {
                    ServerFolderName = stream.KMLData.DataType.Name,
                    ServerFileName = stream.KMLData.FileName,
                    LocalFileName = tempFileName,
                    ContentType = stream.KMLData.DataType.FileType.ContentType
                };

                var useableUrl = this.StorageProvider.Add(addParams);

                if (useableUrl == null)
                {
                    Console.WriteLine("Could not upload file to KML-Useable Storage Provider");
                    return false;
                }

                stream.KMLData.UseableUrl = useableUrl;
            }

            var response = await this.Client.UpdateKMLData(stream.KMLData);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Could not Update KML Data with useable URL");
                return false;
            }

            File.Delete(tempFileName);

            File.Delete(this.FilePath);

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
                    Console.WriteLine("Could not download file for stream");
                    return false;
                }
                else
                {
                    Console.WriteLine("Stream updated!");
                }
            }

            return true;
        }
    }
}
