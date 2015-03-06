using System.ComponentModel.DataAnnotations.Schema;
using WeatherAPIModels.KMLFormatters;
using WeatherAPIModels.Models;

namespace WeatherAPIModels.SpecificModels.KMLDataTypes
{
    [NotMapped]
    public abstract class SpecificKMLDataType : KMLDataType
    {
        public abstract kml GenerateKML(string url);
    }
}
