using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAPIModels;
using WeatherAPIModels.Models;

namespace CAPSWeatherAPI.Contexts
{
    public class WeatherAPIContext : DbContext 
    {
        public WeatherAPIContext() {}
        public DbSet<KMLData> KMLData { get; set; }
        public DbSet<KMLStream> KMLStreams { get; set; }

        public System.Data.Entity.DbSet<KMLDataType> KMLDataTypes { get; set; }

        public System.Data.Entity.DbSet<FileType> FileTypes { get; set; }
    }
}
