using WeatherAPIModels.KMLFormatters;
using WeatherAPIModels.Models;
using WeatherAPIModels.SpecificModels.FileTypes;

namespace WeatherAPIModels.SpecificModels.KMLDataTypes
{
    public class TemperatureKMLDataType : SpecificKMLDataType
    {
        public TemperatureKMLDataType()
        {
            this.Name = "Temperature";
            this.FileType = new KMZFileType();
        }

        public TemperatureKMLDataType(KMLDataType kmlDataType)
        {
            this.Name = "Temperature";
            this.FileType = new KMZFileType();
            this.ID = kmlDataType.ID;
            this.FileTypeID = kmlDataType.FileTypeID;
        }


        public override kml GenerateKML(string url)
        {
            return null;
        }
    }
}
