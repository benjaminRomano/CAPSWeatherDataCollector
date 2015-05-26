using System.ComponentModel.DataAnnotations.Schema;
using WeatherAPIModels.FileTypes;
using WeatherAPIModels.KMLFormatters;
using WeatherAPIModels.Models;

namespace WeatherAPIModels.KMLDataTypes
{
    [NotMapped]
    public class TemperatureKMLDataType : KMLDataType
    {
        public TemperatureKMLDataType(int id = 0, int fileTypeId = 0)
        {
            this.Name = "Temperature";
            this.FileType = new KMZFileType();
            this.Id = id;
            this.FileTypeId = fileTypeId;
        }
    }
}
