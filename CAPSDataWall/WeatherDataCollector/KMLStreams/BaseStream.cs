using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeatherAPIModels;
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

                try
                {
                    using (var webClient = new WebClient())
                    {

                        webClient.DownloadFile(new Uri(stream.KMLData.StorageUrl), tempFileName);
                    }
                }
                catch (WebException)
                {
                    Console.WriteLine("Could not download file from Storage Provider");
                    return false;
                }

                var addParams = new StorageProviderAddParams()
                {
                    ServerFolderName = Enum.GetName(typeof(KMLDataType), stream.Type),
                    ServerFileName = stream.KMLData.FileName,
                    LocalFileName = tempFileName,
                    ContentType = WeatherDataConstants.ContentTypesForData[stream.Type]
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
