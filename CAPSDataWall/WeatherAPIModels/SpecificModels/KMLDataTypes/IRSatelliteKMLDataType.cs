using WeatherAPIModels.KMLFormatters;
using WeatherAPIModels.Models;
using WeatherAPIModels.SpecificModels.FileTypes;

namespace WeatherAPIModels.SpecificModels.KMLDataTypes
{
    public class IRSatelliteKMLDataType : SpecificKMLDataType
    {
        public IRSatelliteKMLDataType()
        {
            this.Name = "IRSatellite";
            this.FileType = new KMZFileType();
        }

        public IRSatelliteKMLDataType(KMLDataType kmlDataType)
        {
            this.Name = "IRSatellite";
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
