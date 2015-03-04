using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using CAPSWeatherAPI.Contexts;
using CAPSWeatherAPI.Extensions;
using WeatherAPIModels;
using WeatherAPIModels.Models;

namespace CAPSWeatherAPI.Services
{
    public static class KMLDataTypeService
    {
        public static async Task<KMLDataType> AddKMLDataType(KMLDataType kmlDataType)
        {
            KMLDataType newKMLDataType;

            using (var context = new WeatherAPIContext())
            {
                if (FileTypeService.FileTypeExists(kmlDataType.FileType.Name))
                {
                    var foundFileType = await FileTypeService.FindFileType(kmlDataType.FileType.Name);
                    kmlDataType.FileTypeID = foundFileType.ID;
                    kmlDataType.FileType = null;
                }

                context.KMLDataTypes.Add(kmlDataType);
                await context.SaveChangesAsync();
                newKMLDataType = await context.CompleteKMLDataTypes().FirstOrDefaultAsync(k => k.ID == kmlDataType.ID);
            }

            return newKMLDataType;
        }

        public static IQueryable<KMLDataType> GetAllKMLDataTypes()
        {
            IQueryable<KMLDataType> allKMLDataTypes;

            using (var context = new WeatherAPIContext())
            {
                allKMLDataTypes = context.CompleteKMLDataTypes();
            }

            return allKMLDataTypes;
        }

        public static async Task<KMLDataType> GetKMLDataType(int id)
        {
            KMLDataType kmlDataType;

            using (var context = new WeatherAPIContext())
            {
                kmlDataType = await context.CompleteKMLDataTypes().FirstOrDefaultAsync(k => k.ID == id);
            }

            return kmlDataType;

        }

        public static async Task<bool> UpdateKMLDataType(KMLDataType kmlDataType)
        {
            var success = true;

            using (var context = new WeatherAPIContext())
            {
                context.Entry(kmlDataType).State = EntityState.Modified;

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KMLDataTypeExists(kmlDataType.Name))
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

        public static async void DeleteKMLDataType(KMLDataType kmlDataType)
        {
            using (var context = new WeatherAPIContext())
            {
                context.KMLDataTypes.Remove(kmlDataType);
                await context.SaveChangesAsync();
            }
        }


        public static async Task<KMLDataType> FindKMLDataType(string name)
        {
            KMLDataType kmlDataType;

            using (var context = new WeatherAPIContext())
            {
                kmlDataType = await context.CompleteKMLDataTypes().Where(e => e.Name == name).FirstOrDefaultAsync();
            }

            return kmlDataType;
        }

        public static bool KMLDataTypeExists(string name)
        {
            bool exists;

            using (var context = new WeatherAPIContext())
            {
                exists = context.KMLDataTypes.Count(e => e.Name == name) > 0;
            }

            return exists;
        }
    }
}
