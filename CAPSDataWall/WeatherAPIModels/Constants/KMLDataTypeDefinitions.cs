using WeatherAPIModels.Models;

namespace WeatherAPIModels.Constants
{
    public static  class KMLDataTypeDefinitions
    {
        public static KMLDataType RadarDataType = new KMLDataType()
        {
            Name = "Radar",
            FileType = FileTypeDefinitions.GifFileType
        };
    }
}
