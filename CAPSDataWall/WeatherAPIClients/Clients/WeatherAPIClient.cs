using System;
using System.Net.Http;
using System.Net.Http.Headers;
using WeatherAPIClients.Requests;

namespace WeatherAPIClients.Clients
{
    public class WeatherAPIClient : IDisposable
    {
        private HttpClient Client { get; set; }
        public FileTypeRequests FileType { get; private set; }
        public KMLDataRequests KMLData { get; private set; }
        public KMLDataTypeRequests KMLDataType { get; private set; }
        public KMLStreamRequests KMLStream { get; private set; }

        public WeatherAPIClient(string apiUri)
        {
            Client = new HttpClient {BaseAddress = new Uri(apiUri) };
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            FileType = new FileTypeRequests(this.Client);
            KMLData = new KMLDataRequests(this.Client);
            KMLDataType = new KMLDataTypeRequests(this.Client);
            KMLStream = new KMLStreamRequests(this.Client);
        }

        public void Dispose()
        {
            this.Client.Dispose();
        }
    }
}
