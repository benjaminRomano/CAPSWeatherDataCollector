using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using WeatherAPIModels.KMLFormatters;
using WeatherAPIModels.Models;
using WeatherAPIModels.StreamDescriptions;
using WeatherDataCollector.StorageProvider;

namespace WeatherDataCollector.Streams
{
    class HistoricalStream : BaseStream
    {
        private TimeSpan MinutesSinceLastCheck { get; set; }

        public HistoricalStream(IKMLUseableStorageProvider storageProvider,KMLStreamDescription inputStreamDescription, KMLStreamDescription outputStreamDescription, string filePath, TimeSpan updateFrequency) :
            base(storageProvider,inputStreamDescription,outputStreamDescription,filePath,updateFrequency)
        {
            this.MinutesSinceLastCheck = new TimeSpan();
        }

        public override void StartStream()
        {
            this.Stream = new Timer(async e =>
            {
                //Check if input status stream data was updated
                var response = await Client.GetKMLStream(this.InputStreamDescription);

                //Update Time since last check
                this.MinutesSinceLastCheck = this.MinutesSinceLastCheck.Add(TimeSpan.FromSeconds(30));


                KMLStream inputKMLStream = null;

                if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Historical Stream failed to get input kml stream!");
                    return;
                }

                if (response.IsSuccessStatusCode)
                {
                    inputKMLStream =  await response.Content.ReadAsAsync<KMLStream>();
                }

                KMLStream outputKMLStream;


                if ((inputKMLStream == null && this.MinutesSinceLastCheck < this.UpdateFrequency) ||
                    (inputKMLStream != null && !inputKMLStream.Updated &&
                     this.MinutesSinceLastCheck < this.UpdateFrequency))
                {
                    return;
                }

                if (inputKMLStream != null && inputKMLStream.Updated)
                {
                    inputKMLStream.Updated = false;
                    await this.Client.UpdateKMLStream(inputKMLStream);

                    //Update data from inputStreamDescription
                    response = await Client.UpdateKMLStream(this.OutputStreamDescription, inputKMLStream.KMLData.ID,false);

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Historical Stream failed to update");
                        return;
                    }

                    outputKMLStream = await response.Content.ReadAsAsync<KMLStream>();
                }
                else
                {
                    this.MinutesSinceLastCheck = TimeSpan.FromMinutes(0);

                    response = await Client.IncrementKMLStream(this.OutputStreamDescription, false);

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Historical Stream failed to update");
                        return;
                    }

                    outputKMLStream = await response.Content.ReadAsAsync<KMLStream>();
                }


                var result = await UpdateLocalStreamFile(outputKMLStream);

                if (!result)
                {
                    Console.WriteLine("Could not write to local history stream file!");
                }

            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        }
    }
}
