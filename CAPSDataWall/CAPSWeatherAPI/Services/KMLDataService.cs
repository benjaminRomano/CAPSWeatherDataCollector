using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAPSWeatherAPI.Contexts;
using CAPSWeatherAPI.Extensions;
using WeatherAPIModels;
using WeatherAPIModels.Models;

namespace CAPSWeatherAPI.Services
{
    public static class KMLDataService
    {

        public static async Task<KMLData> GetDataAtTime(DateTime targetDate, string typeName)
        {
            KMLData kmlData = null;

            using (var context = new WeatherAPIContext())
            {
               var nearestDataTimeDiff = await context.KMLData.Where(c=> c.DataType.Name == typeName).MinAsync(c => SqlFunctions.DateDiff("minute", c.CreatedAt, targetDate));

                if (nearestDataTimeDiff != null)
                {
                    var nearestDiff = nearestDataTimeDiff.GetValueOrDefault();

                    //Only care if diff is less than 30 minutes
                    if (Math.Abs(nearestDiff) <= 30)
                    {
                        kmlData = await context.CompleteKMLData().Where(c => c.DataType.Name == typeName)
                            .FirstAsync(c => SqlFunctions.DateDiff("minute", c.CreatedAt, targetDate) == nearestDiff);
                    }
                }
            }

            return kmlData;
        }

        public static async Task<KMLData> GetNextRecord(KMLData currentKMLData)
        {
            KMLData kmlData;

            using (var context = new WeatherAPIContext())
            {
                kmlData = await context.CompleteKMLData().Where(k => k.ID > currentKMLData.ID && k.DataTypeID == currentKMLData.DataTypeID)
                    .OrderBy(k => k.ID).FirstOrDefaultAsync();
            }

            return kmlData;
        }

        public static async Task<KMLData> GetLatest(string kmlDataTypeName)
        {
            KMLData kmlData;

            using (var context = new WeatherAPIContext())
            {
                kmlData = await context.CompleteKMLData().Where(k => k.DataType.Name == kmlDataTypeName).OrderByDescending(k => k.CreatedAt).FirstOrDefaultAsync();
            }

            return kmlData;
        }

        public static async Task<KMLData> AddKMLData(KMLData kmlData)
        {
            KMLData newKMLData;

            using (var context = new WeatherAPIContext())
            {
                if (KMLDataTypeService.KMLDataTypeExists(kmlData.DataType.Name))
                {
                    var foundDataType = await KMLDataTypeService.FindKMLDataType(kmlData.DataType.Name);
                    kmlData.DataTypeID = foundDataType.ID;
                    kmlData.DataType = null;
                }

                context.KMLData.Add(kmlData);
                await context.SaveChangesAsync();
                newKMLData = await context.CompleteKMLData().FirstOrDefaultAsync(k => k.ID == kmlData.ID);
            }

            return newKMLData;
        }

        public static IQueryable<KMLData> GetAllKMLData()
        {
            IQueryable<KMLData> allKMLData;

            using (var context = new WeatherAPIContext())
            {
                allKMLData = context.CompleteKMLData();
            }

            return allKMLData;
        }

        public static async Task<KMLData> GetKMLData(int id)
        {
            KMLData kmlData;

            using (var context = new WeatherAPIContext())
            {
                kmlData = await context.CompleteKMLData().FirstOrDefaultAsync(k => k.ID == id);
            }

            return kmlData;

        }

        public static async Task<bool> UpdateKMLData(KMLData kmlData)
        {
            var success = true;

            using (var context = new WeatherAPIContext())
            {
                context.Entry(kmlData).State = EntityState.Modified;

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KMLDataExists(kmlData.ID))
                    {
                        success = false;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return success;
        }

        public static async void DeleteKMLData(KMLData kmlData)
        {
            using (var context = new WeatherAPIContext())
            {
                context.KMLData.Remove(kmlData);
                await context.SaveChangesAsync();
            }
        }

        public static bool KMLDataExists(int id)
        {
            bool exists;
            using (var context = new WeatherAPIContext())
            {
                exists = context.KMLData.Count(e => e.ID == id) > 0;
            }
            return exists;
        }
    }
}
