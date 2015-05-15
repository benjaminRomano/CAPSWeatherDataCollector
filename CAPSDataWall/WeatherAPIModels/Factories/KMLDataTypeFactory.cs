using WeatherAPIModels.KMLDataTypes;
using WeatherAPIModels.Models;

namespace WeatherAPIModels.Factories
{
    /// <summary>
    /// Creates correct KMLDataType. Returns null if type is not valid.
    /// </summary>
    public class KMLDataTypeFactory
    {
        public KMLDataType CreateKMLDataType(string type, int id = 0, int fileTypeId = 0)
        {
            switch (type)
            {
                case "Radar":
                    return new RadarKMLDataType(id, fileTypeId);
                case "Temperature":
                    return new TemperatureKMLDataType(id, fileTypeId);
                case "IRSatellite":
                    return new IRSatelliteKMLDataType(id, fileTypeId);
                default:
                    return null;
            }
        }
    }
}
