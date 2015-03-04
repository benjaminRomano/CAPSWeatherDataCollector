using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAPSWeatherAPI.Contexts;
using CAPSWeatherAPI.Extensions;
using WeatherAPIModels;
using WeatherAPIModels.Models;

namespace CAPSWeatherAPI.Services
{
    public static class KMLStreamService
    {
        public static async Task<KMLStream> AddKMLStream(KMLStream kmlStream)
        {
            KMLStream newKMLStream;

            using (var context = new WeatherAPIContext())
            {
                context.KMLStreams.Add(kmlStream);
                await context.SaveChangesAsync();
                newKMLStream = await context.CompleteKMLStreams().FirstOrDefaultAsync(k => k.ID == kmlStream.ID);
            }

            return newKMLStream;
        }

        public static IQueryable<KMLStream> GetAllKMLStreams()
        {
            IQueryable<KMLStream> allKMLStreams;

            using (var context = new WeatherAPIContext())
            {
                allKMLStreams = context.CompleteKMLStreams();
            }

            return allKMLStreams;
        }

        public static async Task<KMLStream> GetKMLStream(int id)
        {
            KMLStream kmlStream;

            using (var context = new WeatherAPIContext())
            {
                kmlStream = await context.CompleteKMLStreams().FirstOrDefaultAsync();
            }

            return kmlStream;

        }

        public static async Task<bool> UpdateKMLStream(KMLStream kmlStream)
        {
            var success = true;

            using (var context = new WeatherAPIContext())
            {
                context.Entry(kmlStream).State = EntityState.Modified;

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KMLStreamExists(kmlStream.ID))
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

        public static async Task<KMLStream> GetSpecificStream(KMLDataSource source, string typeName, string name)
        {
            KMLStream specificStream;

            using (var context = new WeatherAPIContext())
            {
                specificStream =  await context.CompleteKMLStreams().Where(c => c.Source == source && c.KMLData.DataType.Name == typeName && c.Name == name)
                            .FirstOrDefaultAsync();
            }

            return specificStream;
        }

        public static async void DeleteKMLStream(KMLStream kmlStream)
        {
            using (var context = new WeatherAPIContext())
            {
                context.KMLStreams.Remove(kmlStream);
                await context.SaveChangesAsync();
            }
        }

        public static bool KMLStreamExists(int id)
        {
            bool exists;

            using (var context = new WeatherAPIContext())
            {
                exists = context.KMLStreams.Count(e => e.ID == id) > 0;
            }

            return exists;

        }

    }
}
