using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeatherAPIModels;
using WeatherAPIModels.Models;
using WeatherDataCollector.Constants;
using WeatherDataCollector.KMLFormats;
using WeatherDataCollector.Requests;
using WeatherDataCollector.StorageProvider;

namespace WeatherDataCollector.KMLStreams
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
            if (stream.KMLData.UseableUrl == null)
            {
                var tempFileName = Path.GetTempFileName();

                var downloadSuccessful = RequestHelper.DownloadFile(stream.KMLData.StorageUrl, tempFileName);

                if (!downloadSuccessful)
                {
                    return false;
                }

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

                var response = await this.Client.UpdateKMLData(stream.KMLData);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Could not Update KML Data with useable URL");
                    return false;
                }

                File.Delete(tempFileName);
            }

            return true;
        }
    }
}
