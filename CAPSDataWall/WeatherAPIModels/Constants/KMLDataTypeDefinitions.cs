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

        public static KMLDataType TemperatureDataType = new KMLDataType()
        {
            Name = "Temperature",
            FileType = FileTypeDefinitions.KMZFileType
        };
    }
}
