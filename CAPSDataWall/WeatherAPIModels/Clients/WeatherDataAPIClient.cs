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
            var requestUri = String.Concat(WeatherAPIConstants.Root, WeatherAPIConstants.KMLData,"?id=",kmlData.Id);
            return await Client.PutAsJsonAsync(requestUri, kmlData);
        }


        public async Task<HttpResponseMessage> GetKMLStream(StreamDescription streamDescription)
        {
            var requestUri = String.Concat(WeatherAPIConstants.Root, WeatherAPIConstants.KMLStream,"?streamDescription=",streamDescription);
            return await Client.GetAsync(requestUri);


        }

        public async Task<HttpResponseMessage> UpdateKMLStream(StreamDescription streamDescription, int kmlDataId)
        {
            var requestUri = String.Concat(WeatherAPIConstants.Root, WeatherAPIConstants.KMLStream,WeatherAPIConstants.Update,"?streamDescription=",streamDescription,"&kmlDataId=",kmlDataId);
            return await Client.PutAsync(new Uri(requestUri),null);
         
        }

        public async Task<HttpResponseMessage> UpdateKMLStream(KMLStream stream)
        {
            var requestUri = String.Concat(WeatherAPIConstants.Root, WeatherAPIConstants.KMLStream, WeatherAPIConstants.Update,"?id=",stream.Id);
            return await Client.PutAsJsonAsync(requestUri, stream);

        }

        public async Task<HttpResponseMessage> IncrementKMLStream(StreamDescription streamDescription)
        {
            var requestUri = String.Concat(WeatherAPIConstants.Root, WeatherAPIConstants.KMLStream, WeatherAPIConstants.Increment, "?streamDescription=", streamDescription);
            return await Client.PutAsync(new Uri(requestUri), null);
        }

        public void Dispose()
        {
            this.Client.Dispose();
        }


        public async Task<HttpResponseMessage> GetKMLStreamUpdateStatus(StreamDescription streamDescription)
        {
            var requestUri = String.Concat(WeatherAPIConstants.Root, WeatherAPIConstants.KMLStream, WeatherAPIConstants.UpdateStatus,"?streamDescription=",streamDescription);
            return await Client.GetAsync(requestUri);
        }
    }
}
