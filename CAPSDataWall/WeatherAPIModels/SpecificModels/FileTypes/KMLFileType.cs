using WeatherAPIModels.Models;

namespace WeatherAPIModels.SpecificModels.FileTypes
{
    public class KMLFileType : SpecificFileType
    {
        public KMLFileType()
        {
            this.Name = "kml";
            this.ContentType = "application/vnd.google-earth.kml+xml";
            this.RequiresKMLFileCreation = false;
        }

        public KMLFileType(FileType fileType)
        {
            this.Name = "kml";
            this.ContentType = "application/vnd.google-earth.kml+xml";
            this.RequiresKMLFileCreation = false;
            this.ID = fileType.ID;
        }

    }
}
