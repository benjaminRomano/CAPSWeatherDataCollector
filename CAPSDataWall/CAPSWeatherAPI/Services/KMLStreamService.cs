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
                return await this.Context.CompleteKMLStreams().FirstOrDefaultAsync(k => k.ID == kmlStream.ID);
        }

        public IQueryable<KMLStream> GetAllKMLStreams()
        {
            return this.Context.CompleteKMLStreams();
        }

        public async Task<KMLStream> GetKMLStream(int id)
        {
            return await this.Context.CompleteKMLStreams().FirstOrDefaultAsync(k => k.ID == id);
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
                if (!KMLStreamExists(kmlStream.ID))
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

        public async Task<KMLStream> GetSpecificStream(KMLDataSource source, string typeName, string name)
        {
            return await this.Context.CompleteKMLStreams().Where(c => c.Source == source && c.KMLData.DataType.Name == typeName && c.Name == name)
                            .FirstOrDefaultAsync();
        }

        public async void DeleteKMLStream(KMLStream kmlStream)
        {
                this.Context.KMLStreams.Remove(kmlStream);
                await this.Context.SaveChangesAsync();
        }

        public bool KMLStreamExists(int id)
        {
            return this.Context.KMLStreams.Count(e => e.ID == id) > 0;
        }

        public IQueryable<String> GetStreamNames()
        {
            return this.Context.KMLStreams.Select(n => n.Name).Distinct();
        }
    }
}
