using System.ComponentModel.DataAnnotations.Schema;
using WeatherAPIModels.Models;

namespace WeatherAPIModels.FileTypes
{
    public class GifFileType : FileType
    {
        public GifFileType(int id = 0)
        {
            this.Name = "gif";
            this.ContentType = "image/gif";
            this.RequiresKMLFileCreation = true;
            this.Id = id;
        }
    }
}
