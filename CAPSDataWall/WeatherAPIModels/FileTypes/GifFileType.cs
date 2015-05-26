using System.ComponentModel.DataAnnotations.Schema;
using WeatherAPIModels.Models;

namespace WeatherAPIModels.FileTypes
{
    [NotMapped]
    public class GifFileType : FileType
    {
        public GifFileType()
        {
           InitializeProperties(); 
        }
        public GifFileType(int id)
        {
            InitializeProperties();
            this.Id = id;
        }

        private void InitializeProperties()
        {
            this.Name = "gif";
            this.ContentType = "image/gif";
            this.RequiresKMLFileCreation = true;
        }
    }
}
