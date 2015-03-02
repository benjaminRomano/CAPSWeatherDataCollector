using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeatherAPIModels;
using WeatherDataCollector.Constants;
using WeatherDataCollector.KMLFormats;
using WeatherDataCollector.Requests;
using WeatherDataCollector.Responses;
using WeatherDataCollector.StorageProvider;


namespace WeatherDataCollector.KMLStreams
{
    class HistoricalStream : BaseStream
    {
        public HistoricalStream(IKMLUseableStorageProvider storageProvider,KMLStreamDescription inputStreamDescription, KMLStreamDescription outputStreamDescription, string filePath, TimeSpan updateFrequency) :
            base(storageProvider,inputStreamDescription,outputStreamDescription,filePath,updateFrequency)
        {

        }

        public override void StartStream()
        {
            this.Stream = new Timer(async e =>
            {
                //Check if input status stream data was updated
                var response = await Client.GetKMLStream(this.InputStreamDescription);


                KMLStream inputKMLStream = null;

                if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Historical Stream failed to get input kml stream!");
                    return;
                }

                if (response.IsSuccessStatusCode)
                {
                    inputKMLStream = await response.Content.ReadAsAsync<KMLStream>();
                }

                KMLStream outputKMLStream;

                if (inputKMLStream != null && inputKMLStream.Updated)
                {
                    inputKMLStream.Updated = false;
                    this.Client.UpdateStreamStatus(inputKMLStream);

                    //Update data from inputStreamDescription
                    response = await Client.UpdateStreamStatus(this.OutputStreamDescription, inputKMLStream.KMLData.ID);

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Historical Stream failed to update");
                        return;
                    }

                    outputKMLStream = await response.Content.ReadAsAsync<KMLStream>();
                }
                else
                {
                    response = await Client.IncrementKMLStream(this.OutputStreamDescription);

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.Write("Historical Stream failed to update");
                        return;
                    }

                    outputKMLStream = await response.Content.ReadAsAsync<KMLStream>();
                }


                var result = await UpdateLocalStreamFile(outputKMLStream);

                if (!result)
                {
                    Console.WriteLine("Could not write to local history stream file!");
                }

                Console.WriteLine("Updating {0} with historical data", this.FilePath);
                File.Delete(this.FilePath);
                KMLFileCreator.CreateKMLFile(outputKMLStream.Type, outputKMLStream.KMLData.UseableUrl, this.FilePath);

            }, null, TimeSpan.Zero, this.UpdateFrequency);

        }
    }
}
