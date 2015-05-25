using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherAPIModels.Models;

namespace WeatherAPIClients.Requests
{
    public class FileTypeRequests
    {
        /// <summary>
        /// Requires client with base address already set
        /// </summary>
        private HttpClient Client { get; set; }

        private string FileTypeUri
        {
            get { return "FileType/"; }
        }

        public FileTypeRequests(HttpClient client)
        {
            this.Client = client;
        }

        public async Task<HttpResponseMessage> GetFileTypes()
        {
            return await Client.GetAsync(this.FileTypeUri);
        }

        public async Task<HttpResponseMessage> GetFileType(int id)
        {
            var requestUri = String.Concat(this.FileTypeUri, id);
            return await Client.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> PutFileType(int id, FileType fileType)
        {
            var requestUri = String.Concat(this.FileTypeUri,id);
            return await Client.PutAsJsonAsync(requestUri, fileType);
        }

        public async Task<HttpResponseMessage> PostFileType(FileType fileType)
        {
            return await Client.PostAsJsonAsync(this.FileTypeUri, fileType);
        }

        public async Task<HttpResponseMessage> DeleteFileType(int id)
        {
            var requestUri = String.Concat(this.FileTypeUri,id);
            return await Client.DeleteAsync(requestUri);
        } 
    }
}