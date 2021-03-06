﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAPSWeatherAPI.Contexts;
using WeatherAPIModels.Models;

namespace CAPSWeatherAPI.Extensions
{
    /// <summary>
    /// Extensions for getting complete models
    /// </summary>
    public static class ModelExtensions
    {
        public static IQueryable<KMLData> CompleteKMLData(this WeatherDataContext context)
        {
            return context.KMLData.Include(k => k.DataType).Include(k => k.DataType.FileType);
        }

        public static IQueryable<KMLStream> CompleteKMLStreams(this WeatherDataContext context)
        {
            return context.KMLStreams.Include(k => k.KMLData)
                    .Include(k => k.KMLData.DataType)
                    .Include(k => k.KMLData.DataType.FileType);
        }

        public static IQueryable<KMLDataType> CompleteKMLDataTypes(this WeatherDataContext context)
        {
            return context.KMLDataTypes.Include(k => k.FileType);
        }

        public static IQueryable<FileType> CompleteFileTypes(this WeatherDataContext context)
        {
            return context.FileTypes;
        }
    }
}
