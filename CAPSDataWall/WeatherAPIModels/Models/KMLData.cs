using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherAPIModels.Models
{
    public class KMLData
    {
        public int ID { get; set; }
        public string StorageUrl { get; set; }
        public string UseableUrl { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedAt { get; set; }

        public int DataTypeID { get; set; }

        [ForeignKey("DataTypeID")]
        public virtual KMLDataType DataType { get; set; }
    }
}
