using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WeatherAPIModels.Constants;
using WeatherAPIModels.Models;
using WeatherAPIModels.StreamDescriptions;

namespace WeatherAPIModels.Clients
{
    public class WeatherDataAPIClient : IDisposable
    {
        private HttpClient Client { get; set; }

        public WeatherDataAPIClient()
        {
            Client = new HttpClient {BaseAddress = new Uri(WeatherAPIConstants.Root) };
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        }

        public Task<HttpResponseMessage> AddKMLData(KMLData kmlData)
        {
            var requestUri = String.Concat(WeatherAPIConstants.Root, WeatherAPIConstants.KMLData);

            return Client.PostAsJsonAsync(requestUri, kmlData);
        }

        public async Task<HttpResponseMessage> UpdateKMLData(KMLData kmlData)
        {
            var requestUri = String.Concat(WeatherAPIConstants.Root, WeatherAPIConstants.KMLData,"?id=",kmlData.ID);
            return await Client.PutAsJsonAsync(requestUri, kmlData);
        }


        public async Task<HttpResponseMessage> GetKMLStream(KMLStreamDescription stream)
        {
            var requestUri = String.Concat(WeatherAPIConstants.Root, WeatherAPIConstants.KMLStream,"?source=",stream.Source,"&typeName=",stream.KMLDataType.Name,"&name=",stream.StreamName);
            return await Client.GetAsync(requestUri);


        }

        public async Task<HttpResponseMessage> UpdateKMLStream(KMLStreamDescription stream, int kmlDataId, bool setUpdated)
        {
            var requestUri = String.Concat(WeatherAPIConstants.Root, WeatherAPIConstants.KMLStream,WeatherAPIConstants.Update,"?source=",stream.Source,"&kmlDataId=",kmlDataId,"&name=",stream.StreamName, "&setUpdated=",setUpdated);
            return await Client.PutAsync(new Uri(requestUri),null);
         
        }

        public async Task<HttpResponseMessage> UpdateKMLStream(KMLStream stream)
        {
            var requestUri = String.Concat(WeatherAPIConstants.Root, WeatherAPIConstants.KMLStream, WeatherAPIConstants.Update,"?id=",stream.ID);
            return await Client.PutAsJsonAsync(requestUri, stream);

        }

        public async Task<HttpResponseMessage> IncrementKMLStream(KMLStreamDescription stream, bool setUpdated)
        {
            var requestUri = String.Concat(WeatherAPIConstants.Root, WeatherAPIConstants.KMLStream, WeatherAPIConstants.Increment, "?source=", stream.Source,"&typeName=",stream.KMLDataType.Name, "&name=", stream.StreamName,"&SetUpdated=",setUpdated);
            return await Client.PutAsync(new Uri(requestUri), null);
        }

        public void Dispose()
        {
            this.Client.Dispose();
        }

    }
}
