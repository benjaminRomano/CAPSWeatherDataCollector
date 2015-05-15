using System.ComponentModel.DataAnnotations.Schema;
using WeatherAPIModels.Models;

namespace WeatherAPIModels.FileTypes
{
    public class KMZFileType : FileType
    {
        public KMZFileType(int id = 0)
        {
            this.Name = "kmz";
            this.ContentType = "application/vnd.google-earth.kmz";
            this.RequiresKMLFileCreation = false;
            this.Id = id;
        }

    }

}

