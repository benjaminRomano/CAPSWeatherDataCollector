using System.ComponentModel.DataAnnotations.Schema;
using WeatherAPIModels.Models;

namespace WeatherAPIModels.FileTypes
{
    public class KMLFileType : FileType
    {
        public KMLFileType(int id = 0)
        {
            this.Name = "kml";
            this.ContentType = "application/vnd.google-earth.kml+xml";
            this.RequiresKMLFileCreation = false;
            this.Id = id;
        }
    }
}
