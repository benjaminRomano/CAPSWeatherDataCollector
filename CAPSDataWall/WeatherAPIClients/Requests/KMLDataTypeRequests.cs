using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherAPIModels.Models;

namespace WeatherAPIClients.Requests
{
    public class KMLDataTypeRequests
    {
        /// <summary>
        /// Requires client with base address already set
        /// </summary>
        private HttpClient Client { get; set; }

        private string KMLDataTypeUri
        {
            get { return "KMLDataType/"; }
        }

        public KMLDataTypeRequests(HttpClient client)
        {
            this.Client = client;
        }

        public async Task<HttpResponseMessage> GetKMLDataTypes()
        {
            return await Client.GetAsync(this.KMLDataTypeUri);
        }

        public async Task<HttpResponseMessage> GetKMLDataType(int id)
        {
            var requestUri = String.Concat(this.KMLDataTypeUri, id);
            return await Client.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> PutKMLDataType(int id, KMLDataType kmlDataType)
        {
            var requestUri = String.Concat(this.KMLDataTypeUri, id);
            return await Client.PutAsJsonAsync(requestUri, kmlDataType);
        }

        public async Task<HttpResponseMessage> PostKMLDataType(KMLDataType kmlDataType)
        {
            return await Client.PostAsJsonAsync(this.KMLDataTypeUri, kmlDataType);
        }

        public async Task<HttpResponseMessage> DeleteKMLDataType(int id)
        {
            var requestUri = String.Concat(this.KMLDataTypeUri, id);
            return await Client.DeleteAsync(requestUri);
        }
    }
}