using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using WeatherAPIModels.KMLFormatters;

namespace WeatherAPIModels.Models
{
    public class KMLDataType
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(200)]
        public string Name { get; set; }

        public int FileTypeId { get; set; }

        [ForeignKey("FileTypeId")]
        public FileType FileType { get; set; }

    }
}
