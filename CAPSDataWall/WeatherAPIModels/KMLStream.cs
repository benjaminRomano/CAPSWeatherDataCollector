using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAPIModels
{
    public class KMLStream
    {
        public int ID { get; set; }
        public KMLDataSource Source { get; set; }
        public KMLDataType Type { get; set; }
        public string Name { get; set; }
        public bool Updated { get; set; }

        public virtual KMLData KMLData { get; set; }
    }
}
