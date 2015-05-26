using System.ComponentModel.DataAnnotations.Schema;
using WeatherAPIModels.Models;

namespace WeatherAPIModels.FileTypes
{
    [NotMapped]
    public class KMLFileType : FileType
    {
        public KMLFileType()
        {
            InitializeProperties();
        }

        public KMLFileType(int id)
        {
            InitializeProperties();
            this.Id = id;
        }

        private void InitializeProperties()
        {
            this.Name = "kml";
            this.ContentType = "application/vnd.google-earth.kml+xml";
            this.RequiresKMLFileCreation = false;
        }
    }
}
