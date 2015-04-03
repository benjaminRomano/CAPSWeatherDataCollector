using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAPIModels.Models;
using WeatherAPIModels.SpecificModels.FileTypes;
using WeatherAPIModels.SpecificModels.KMLDataTypes;

namespace WeatherAPIModels.ConvertServices
{
    /// <summary>
    /// Use this to convert from general to Specific Object
    /// This allows you to add new types without restarting API
    /// </summary>
    public class SpecificConverterService
    {
        public SpecificKMLDataType ConvertToSpecificKMLData(KMLDataType dataType)
        {
            switch (dataType.Name)
            {
                case "Radar":
                    return new RadarKMLDataType(dataType);
                case "Temperature":
                    return new TemperatureKMLDataType(dataType);
                case "IRSatellite":
                    return new IRSatelliteKMLDataType(dataType);
            }

            return new BaseKMLDataType(dataType);
        }

        public SpecificFileType ConvertToSpecificFileType(KMLFileType fileType)
        {
            switch (fileType.Name)
            {
                case "gif":
                    return new GifFileType(fileType);
                case "kml":
                    return new KMLFileType(fileType);
                case "kmz":
                    return new KMZFileType(fileType);
            }

            return new BaseFileType(fileType);
        }
    }
}
