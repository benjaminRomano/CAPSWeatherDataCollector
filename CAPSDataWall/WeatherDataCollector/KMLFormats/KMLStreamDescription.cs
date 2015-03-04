using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAPIModels;
using WeatherAPIModels.Models;

namespace WeatherDataCollector.KMLFormats
{
    public class KMLStreamDescription
    {
        public KMLDataSource Source { get; set; }
        public KMLDataType Type { get; set; }
        public string Name { get; set; }

        public KMLStreamDescription(KMLDataSource source, KMLDataType type, string name)
        {
            this.Source = source;
            this.Type = type;
            this.Name = name;
        }
    }
}
