using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WeatherAPIModels;
using WeatherDataCollector.Constants;
using WeatherDataCollector.KMLFormats;

namespace WeatherDataCollector.Requests
{
    public class WeatherDataAPIClient : IDisposable
    {
        private HttpClient Client { get; set; }

        public WeatherDataAPIClient()
        {
            Client = new HttpClient {BaseAddress = new Uri(WeatherAPIConstants.Root) };
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(WeatherDataConstants.JsonContent));
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
            var requestUri = String.Concat(WeatherAPIConstants.Root, WeatherAPIConstants.KMLStream,"?source=",stream.Source,"&type=",stream.Type,"&name=",stream.Name);
            return await Client.GetAsync(requestUri);


        }

        public async Task<HttpResponseMessage> UpdateStreamStatus(KMLStreamDescription stream, int id)
        {
            var requestUri = String.Concat(WeatherAPIConstants.Root, WeatherAPIConstants.KMLStream,WeatherAPIConstants.Update,"?source=",stream.Source,"&id=",id,"&name=",stream.Name);
            return await Client.PutAsync(new Uri(requestUri),null);
         
        }

        public async Task<HttpResponseMessage> UpdateStreamStatus(KMLStream streamStatus)
        {
            var requestUri = String.Concat(WeatherAPIConstants.Root, WeatherAPIConstants.KMLStream, WeatherAPIConstants.Update,"?id=",streamStatus.ID);
            return await Client.PutAsJsonAsync(requestUri, streamStatus);

        }

        public async Task<HttpResponseMessage> IncrementKMLStream(KMLStreamDescription stream)
        {
            var requestUri = String.Concat(WeatherAPIConstants.Root, WeatherAPIConstants.KMLStream, WeatherAPIConstants.Increment, "?source=", stream.Source,"&type=",stream.Type, "&name=", stream.Name);
            return await Client.PutAsync(new Uri(requestUri), null);
        }

        public void Dispose()
        {
            this.Client.Dispose();
        }
    }
}
