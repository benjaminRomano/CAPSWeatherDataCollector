using System.ComponentModel.DataAnnotations.Schema;
using WeatherAPIModels.FileTypes;
using WeatherAPIModels.KMLFormatters;
using WeatherAPIModels.Models;

namespace WeatherAPIModels.KMLDataTypes
{
    [NotMapped]
    public class RadarKMLDataType : KMLDataType
    {
        public RadarKMLDataType(int id = 0, int fileTypeId = 0)
        {
            this.Name = "Radar";
            this.FileType = new GifFileType();
            this.Id = id;
            this.FileTypeId = fileTypeId;
        }
    }
}
