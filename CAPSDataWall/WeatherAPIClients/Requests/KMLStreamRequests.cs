using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherAPIModels.Models;
using WeatherAPIModels.Utilities;

namespace WeatherAPIClients.Requests
{
    public class KMLStreamRequests
    {
        /// <summary>
        /// Requires client with base address already set
        /// </summary>
        private HttpClient Client { get; set; }

        private string KMLStreamUri
        {
            get { return "KMLStream/"; }
        }

        public KMLStreamRequests(HttpClient client)
        {
            this.Client = client;
        }

        public async Task<HttpResponseMessage> GetKMLStreams()
        {
            return await Client.GetAsync(this.KMLStreamUri);
        }

        public async Task<HttpResponseMessage> GetStreamNames()
        {
            var requestUri = String.Concat(this.KMLStreamUri, "Names");
            return await Client.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> GetDataTypes(string name)
        {
            var requestUri = String.Concat(this.KMLStreamUri, "DataTypes?name=", name);
            return await Client.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> GetKMLStream(int id)
        {
            var requestUri = String.Concat(this.KMLStreamUri, id);
            return await Client.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> GetKMLStream(StreamDescription description)
        {
            var requestUri = String.Concat(this.KMLStreamUri, "?dataTypeName=", description.DataTypeName,
                "&streamName=", description.StreamName);
            return await Client.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> IncrementKMLStream(StreamDescription description)
        {
            var requestUri = String.Concat(this.KMLStreamUri, "Increment?dataTypeName=", description.DataTypeName,
                "&streamName=", description.StreamName);
            return await Client.PutAsync(requestUri, null);
        }

        public async Task<HttpResponseMessage> PutKMLStream(int kmlDataId, StreamDescription description)
        {
            var requestUri = String.Concat(this.KMLStreamUri, "?id=", kmlDataId, "&dataTypeName=",
                description.DataTypeName, "&streamName=", description.StreamName);
            return await Client.PutAsync(requestUri, null);
        }

        public async Task<HttpResponseMessage> PutKMLStream(int id, KMLStream stream)
        {
            var requestUri = String.Concat(this.KMLStreamUri, id);
            return await Client.PutAsJsonAsync(requestUri, stream);
        }

        public async Task<HttpResponseMessage> PostKMLStream(KMLStream stream)
        {
            return await Client.PostAsJsonAsync(this.KMLStreamUri, stream);
        }

        public async Task<HttpResponseMessage> DeleteKMLStream(int id)
        {
            var requestUri = String.Concat(this.KMLStreamUri, id);
            return await Client.DeleteAsync(requestUri);
        }
    }
}