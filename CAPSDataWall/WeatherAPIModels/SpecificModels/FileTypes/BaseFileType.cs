using WeatherAPIModels.Models;

namespace WeatherAPIModels.SpecificModels.FileTypes
{
    class BaseFileType : SpecificFileType
    {
        public BaseFileType() { }

        public BaseFileType(FileType fileType)
        {
            this.ContentType = fileType.ContentType;
            this.ID = fileType.ID;
            this.Name = fileType.Name;
            this.RequiresKMLFileCreation = fileType.RequiresKMLFileCreation;
        }
    }
}
