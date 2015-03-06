using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherDataCollector.Constants;
using WeatherDataCollector.Responses;

namespace WeatherDataCollector.StorageProvider
{
    internal class ImgurStorageProvider : IKMLUseableStorageProvider 
    {
        private string ClientId { get; set; }


    public ImgurStorageProvider(string clientId)
        {
            this.ClientId = clientId;

        }

        public string Add(StorageProviderAddParams addParams)
        {
            var values = new NameValueCollection
                {
                    { "image", Convert.ToBase64String(File.ReadAllBytes(addParams.LocalFileName)) }
                };

            try
            {
                using (var client = new WebClient())
                {
                    client.Headers.Add(HttpRequestHeader.Authorization, "Client-ID " + ClientId);

                    var response = client.UploadValues(ImgurConstants.Root + ImgurConstants.Upload, values);

                    using (var reader = new StreamReader(new MemoryStream(response), Encoding.Default))
                    {
                        var successResponse = new JsonSerializer().Deserialize<ImgurUploadSuccessResponse>(new JsonTextReader(reader));
                        return successResponse.Data.Link;
                    }
                }
            }
            catch (WebException e)
            {
                using (var reader = new StreamReader(e.Response.GetResponseStream()))
                {
                    var errorResponse = new JsonSerializer().Deserialize<ImgurUploadErrorResponse>(new JsonTextReader(reader));
                    Console.WriteLine(errorResponse.Data.Error);
                    return null;
                }
            }
        }
    }

}
