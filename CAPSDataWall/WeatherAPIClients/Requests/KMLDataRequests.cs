using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherAPIModels.Models;

namespace WeatherAPIClients.Requests
{
    public class KMLDataRequests
    {
        /// <summary>
        /// Requires client with base address already set
        /// </summary>
        private HttpClient Client { get; set; }

        private string KMLDataUri
        {
            get { return "KMLData/"; }
        }

        public KMLDataRequests(HttpClient client)
        {
            this.Client = client;
        }

        public async Task<HttpResponseMessage> GetDataAtTime(string dateString, string typeName)
        {
            var requestUri = String.Concat(this.KMLDataUri, "?dateString=", dateString, "&typeName=", typeName);
            return await Client.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> GetKMLData()
        {
            return await Client.GetAsync(this.KMLDataUri);
        }

        public async Task<HttpResponseMessage> GetKMLData(int id)
        {
            var requestUri = String.Concat(this.KMLDataUri, id);
            return await Client.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> PutKMLData(KMLData kmlData)
        {
            var requestUri = String.Concat(this.KMLDataUri, kmlData.Id);
            return await Client.PutAsJsonAsync(requestUri, kmlData);
        }

        public async Task<HttpResponseMessage> PostKMLData(KMLData kmlData)
        {
            return await Client.PostAsJsonAsync(this.KMLDataUri, kmlData);
        }

        public async Task<HttpResponseMessage> DeleteKMLData(int id)
        {
            var requestUri = String.Concat(this.KMLDataUri, id);
            return await Client.DeleteAsync(requestUri);
        }
    }
}