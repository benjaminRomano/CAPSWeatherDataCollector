using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAPSWeatherAPI.Services;
using WeatherAPIModels;
using WeatherAPIModels.Models;

namespace CAPSWeatherAPI.Contexts
{
    public class WeatherDataContext : DbContext
    {

        public void WeatherAPIContext()
        {

        }

        public DbSet<KMLData> KMLData { get; set; }
        public DbSet<KMLStream> KMLStreams { get; set; }
        public DbSet<KMLDataType> KMLDataTypes { get; set; }
        public DbSet<FileType> FileTypes { get; set; }
    }
}
