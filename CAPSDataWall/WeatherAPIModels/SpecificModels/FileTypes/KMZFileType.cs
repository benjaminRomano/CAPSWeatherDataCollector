using WeatherAPIModels.Models;

namespace WeatherAPIModels.SpecificModels.FileTypes
{
    public class KMZFileType : SpecificFileType
    {
        public KMZFileType()
        {
            this.Name = "kmz";
            this.ContentType = "application/vnd.google-earth.kmz";
            this.RequiresKMLFileCreation = false;
        }

        public KMZFileType(FileType fileType)
        {
            this.Name = "kmz";
            this.ContentType = "application/vnd.google-earth.kmz";
            this.RequiresKMLFileCreation = false;
            this.ID = fileType.ID;
        }

    }
}
