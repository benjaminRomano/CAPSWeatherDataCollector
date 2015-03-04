using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherAPIModels.Models
{
    public class FileType
    {
        public int ID { get; set; }
        [Index(IsUnique=true)]
        [StringLength(200)]
        public string Name { get; set; }
        public string ContentType  { get; set; }
        public bool RequiresKMLFileCreation { get; set; }
    }
}
