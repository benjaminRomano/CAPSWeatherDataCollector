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
    public class WeatherAPIContext : IDisposable
    {
        private WeatherDataContext DataContext;

        public FileTypeService FileTypeService { get; private set; }
        public KMLDataTypeService KMLDataTypeService { get; private set; }
        public KMLStreamService KMLStreamService { get; private set; }
        public KMLDataService KMLDataService { get; private set; }

        public WeatherAPIContext()
        {
            this.DataContext = new WeatherDataContext();
            this.FileTypeService = new FileTypeService(this.DataContext);
            this.KMLDataTypeService = new KMLDataTypeService(this.DataContext);
            this.KMLStreamService = new KMLStreamService(this.DataContext);
            this.KMLDataService = new KMLDataService(this.DataContext);

        }

        public virtual void Dispose()
        {
           this.DataContext.Dispose();
        }
    }
}
