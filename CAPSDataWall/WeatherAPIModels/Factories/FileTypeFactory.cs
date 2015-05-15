using WeatherAPIModels.FileTypes;
using WeatherAPIModels.Models;

namespace WeatherAPIModels.Factories
{
    /// <summary>
    /// Creates correct FileType. Returns null if type is not valid.
    /// </summary>
    public class FileTypeFactory
    {
        public FileType CreateFileType(string type, int id = 0)
        {
            switch (type)
            {
                case "gif":
                    return new GifFileType(id);
                case "kml":
                    return new KMLFileType(id);
                case "kmz":
                    return new KMZFileType(id);
                default:
                    return null;
            }
        }
    }
}
