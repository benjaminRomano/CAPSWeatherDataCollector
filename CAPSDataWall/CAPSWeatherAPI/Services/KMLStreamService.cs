using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using CAPSWeatherAPI.Contexts;
using CAPSWeatherAPI.Extensions;
using WeatherAPIModels;
using WeatherAPIModels.Models;
using WeatherAPIModels.Utilities;

namespace CAPSWeatherAPI.Services
{
    public class KMLStreamService
    {
        private WeatherDataContext Context { get; set; }

        public KMLStreamService(WeatherDataContext context)
        {
            this.Context = context;
        }

        public async Task<KMLStream> AddKMLStream(KMLStream kmlStream)
        {
                this.Context.KMLStreams.Add(kmlStream);
                await this.Context.SaveChangesAsync();
                return await this.Context.CompleteKMLStreams().FirstOrDefaultAsync(k => k.Id == kmlStream.Id);
        }

        public IQueryable<KMLStream> GetAllKMLStreams()
        {
            return this.Context.CompleteKMLStreams();
        }

        public async Task<KMLStream> GetKMLStream(int id)
        {
            return await this.Context.CompleteKMLStreams().FirstOrDefaultAsync(k => k.Id == id);
        }

        public async Task<KMLStream> GetKMLStream(StreamDescription description)
        {
            return await this.Context.CompleteKMLStreams()
                .FirstOrDefaultAsync(k => k.KMLData.DataType.Name == description.KMLDataTypeName && k.Name == description.StreamName);
        }

        public async Task<bool> UpdateKMLStream(KMLStream kmlStream)
        {
            var success = true;

            this.Context.Entry(kmlStream).State = EntityState.Modified;

            try
            {
                await this.Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KMLStreamExists(kmlStream.Id))
                {
                    success = false;
                }
                else
                {
                    throw;
                }
            }

            return success;
        }

        public async void DeleteKMLStream(KMLStream kmlStream)
        {
            this.Context.KMLStreams.Remove(kmlStream);
            await this.Context.SaveChangesAsync();
        }

        public bool KMLStreamExists(int id)
        {
            return this.Context.KMLStreams.Count(k => k.Id == id) > 0;
        }

        public IQueryable<String> GetStreamNames()
        {
            return this.Context.KMLStreams.Select(k => k.Name).Distinct();
        }

        public IQueryable<string> GetDataTypes(string name)
        {
            return this.Context.KMLStreams.Where(k => k.Name == name)
                .Select(k => k.KMLData.DataType.Name).Distinct();
        }

    }
}
