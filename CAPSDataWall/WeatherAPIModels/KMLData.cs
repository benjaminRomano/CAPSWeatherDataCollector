using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAPIModels
{
    public class KMLData
    {
        public int ID { get; set; }
        public string StorageUrl { get; set; }
        public string UseableUrl { get; set; }
        public string FileName { get; set; }
        public KMLDataType Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
