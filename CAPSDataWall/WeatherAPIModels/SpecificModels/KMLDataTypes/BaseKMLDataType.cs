using System.ComponentModel.DataAnnotations.Schema;
using WeatherAPIModels.KMLFormatters;
using WeatherAPIModels.Models;

namespace WeatherAPIModels.SpecificModels.KMLDataTypes
{
    [NotMapped]
    public sealed class BaseKMLDataType : SpecificKMLDataType
    {
        public BaseKMLDataType() { }

        public BaseKMLDataType(KMLDataType kmlDataType)
        {
            this.FileType = kmlDataType.FileType;
            this.FileTypeID = kmlDataType.FileTypeID;
            this.ID = kmlDataType.ID;
            this.Name = kmlDataType.Name;
        }

        public override kml GenerateKML(string url)
        {
            return null;
        }
    }
}
