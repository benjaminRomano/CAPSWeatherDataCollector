using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherAPIModels.Models
{
    public class KMLData 
    {
        public int Id { get; set; }
        public string StorageUrl { get; set; }
        public string UseableUrl { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedAt { get; set; }

        public int DataTypeId { get; set; }

        [ForeignKey("DataTypeId")]
        public KMLDataType DataType { get; set; }
    }
}
