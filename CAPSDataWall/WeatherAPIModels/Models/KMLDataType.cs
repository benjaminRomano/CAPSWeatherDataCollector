using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherAPIModels.Models
{
    public class KMLDataType
    {
        public int ID { get; set; }
        [Index(IsUnique = true)]
        [StringLength(200)]
        public string Name { get; set; }

        public int FileTypeID { get; set; }

        [ForeignKey("FileTypeID")]
        public virtual FileType FileType { get; set; }
    }
}
