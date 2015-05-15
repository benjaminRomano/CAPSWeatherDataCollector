using System;
using System.Net.Http;
using System.Threading;
using WeatherAPIModels.Clients;
using WeatherAPIModels.Models;
using WeatherAPIModels.StreamDescriptions;
using WeatherDataCollector.StorageProvider;

namespace WeatherDataCollector.StreamUpdaters
{
    public class StreamUpdater : IStreamUpdater
    {
        private Timer StreamUpdateTimer { get; set; }
        private WeatherDataAPIClient Client { get; set; }
        private StreamDescription StreamDescription { get; set; }
        private TimeSpan UpdateFrequency { get; set; }

        public StreamUpdater(StreamDescription streamDescription, TimeSpan updateFrequency)
        {
            this.StreamDescription = streamDescription;
            this.UpdateFrequency = updateFrequency;

            this.Client = new WeatherDataAPIClient();

            this.StreamUpdateTimer = null;
        }

        public void Start()
        {
            if (this.StreamUpdateTimer != null)
            {
                return;
            }

            this.StreamUpdateTimer = new Timer(async e =>
            {
                var response = await Client.IncrementKMLStream(this.StreamDescription);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Stream failed to update");
                    return;
                }

            }, null, TimeSpan.Zero, this.UpdateFrequency);
        }

        public void Stop()
        {
            if (this.StreamUpdateTimer != null)
            {
                this.StreamUpdateTimer.Dispose();
            }

            this.StreamUpdateTimer = null;
        }
    }
}
