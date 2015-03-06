using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using WeatherAPIModels.KMLFormatters;
using WeatherAPIModels.Models;
using WeatherAPIModels.StreamDescriptions;
using WeatherDataCollector.Requests;
using WeatherDataCollector.StorageProvider;

namespace WeatherDataCollector.Streams
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
                response = await this.Client.UpdateKMLStream(this.OutputStreamDescription, 
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

            }, null, TimeSpan.Zero, this.UpdateFrequency);
        }
    }
}
