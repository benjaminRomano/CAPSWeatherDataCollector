using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDataCollector.Responses
{
    public class ImgurUploadErrorResponse
    {
        public ImgurUploadErrorData Data { get; set; }
        public bool Success { get; set; }
        public int Status { get; set; }
    }

    public class ImgurUploadErrorData
    {
        public string Error { get; set; }
        public string Request { get; set; }
        public string Method { get; set; }
    }

    public class ImgurUploadSuccessResponse
    {
        public ImgurUploadSuccessData Data { get; set; }
        public bool Success { get; set; }
        public int Status { get; set; }
    }

    public class ImgurUploadSuccessData
    {
        public string ID { get; set; }
        public object Title { get; set; }
        public object Description { get; set; }
        public int Datetime { get; set; }
        public string Type { get; set; }
        public bool Animated { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Size { get; set; }
        public int Views { get; set; }
        public int Bandwidth { get; set; }
        public object Vote { get; set; }
        public bool Favorite { get; set; }
        public object Nsfw { get; set; }
        public object Section { get; set; }
        public object Account_url { get; set; }
        public int Account_id { get; set; }
        public string Deletehash { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
    }

}
