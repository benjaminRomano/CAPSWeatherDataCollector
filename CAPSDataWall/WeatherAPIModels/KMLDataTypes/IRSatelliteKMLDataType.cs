﻿using System.ComponentModel.DataAnnotations.Schema;
using WeatherAPIModels.FileTypes;
using WeatherAPIModels.KMLFormatters;
using WeatherAPIModels.Models;

namespace WeatherAPIModels.KMLDataTypes
{
    [NotMapped]
    public class IRSatelliteKMLDataType : KMLDataType
    {
        public IRSatelliteKMLDataType(int id = 0, int fileTypeId = 0)
        {
            this.Name = "IRSatellite";
            this.FileType = new KMZFileType();
            this.Id = id;
            this.FileTypeId = fileTypeId;
        }
    }
}
