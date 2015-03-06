using WeatherAPIModels.Models;

namespace WeatherAPIModels.SpecificModels.FileTypes
{
    public class GifFileType : SpecificFileType
    {
        public GifFileType()
        {
            this.Name = "gif";
            this.ContentType = "image/gif";
            this.RequiresKMLFileCreation = true;
        }

        public GifFileType(FileType fileType)
        {
            this.Name = "gif";
            this.ContentType = "image/gif";
            this.RequiresKMLFileCreation = true;
            this.ID = fileType.ID;
        }

    }
}
