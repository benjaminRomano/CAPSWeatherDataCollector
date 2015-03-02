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

namespace WeatherDataCollector.KMLStreams
{
    public class LatestStream : BaseStream
    {
        public LatestStream(IKMLUseableStorageProvider storageProvider,KMLStreamDescription inputStreamDescription,KMLStreamDescription outputStreamDescription, string filePath, TimeSpan updateFrequency) :
            base(storageProvider,inputStreamDescription,outputStreamDescription,filePath,updateFrequency)
        {
        }

        public override void StartStream()
        {
            Stream = new Timer(async e =>
            {

                //Get latest data from input KML Stream
                var response = await this.Client.GetKMLStream(this.InputStreamDescription);


                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Could not load stream data");
                    return;
                }

                var inputKMLStream = await response.Content.ReadAsAsync<KMLStream>();


                //Update output KML Stream with input KML Data
                response = await this.Client.UpdateStreamStatus(this.OutputStreamDescription, 
                    inputKMLStream.KMLData.ID);


                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Could not update latest output stream");
                    return;
                }

                var outputKMLStream = await response.Content.ReadAsAsync<KMLStream>();


                var result = await UpdateLocalStreamFile(outputKMLStream);

                if (!result)
                {
                    Console.WriteLine("Could not write to local latest stream file!");
                }

                Console.WriteLine("Updating {0} with latest data", this.FilePath);
                File.Delete(this.FilePath);
                KMLFileCreator.CreateKMLFile(outputKMLStream.Type, outputKMLStream.KMLData.UseableUrl, this.FilePath);


            }, null, TimeSpan.Zero, this.UpdateFrequency);
        }
    }
}
