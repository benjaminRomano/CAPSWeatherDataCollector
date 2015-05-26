using System.ComponentModel.DataAnnotations.Schema;
using WeatherAPIModels.Models;

namespace WeatherAPIModels.FileTypes
{
    [NotMapped]
    public class KMZFileType : FileType
    {
        public KMZFileType()
        {
            InitializeProperties();
        } 

        public KMZFileType(int id)
        {
            InitializeProperties();
            this.Id = id;
        }

        private void InitializeProperties()
        {
            this.Name = "kmz";
            this.ContentType = "application/vnd.google-earth.kmz";
            this.RequiresKMLFileCreation = false;
        }
    }

}

