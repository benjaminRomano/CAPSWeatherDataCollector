using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using CAPSWeatherAPI.Contexts;
using CAPSWeatherAPI.Extensions;
using WeatherAPIModels.Models;

namespace CAPSWeatherAPI.Services
{
    public class KMLDataTypeService
    {
        private WeatherDataContext Context { get; set; }

        public KMLDataTypeService(WeatherDataContext context)
        {
            this.Context = context;
        }

        public async Task<KMLDataType> AddKMLDataType(KMLDataType kmlDataType)
        {
            this.Context.KMLDataTypes.Add(kmlDataType);
            await this.Context.SaveChangesAsync();
            return await this.Context.CompleteKMLDataTypes().FirstOrDefaultAsync(k => k.Id == kmlDataType.Id);
        }

        public IQueryable<KMLDataType> GetAllKMLDataTypes()
        {
             return this.Context.CompleteKMLDataTypes();
        }

        public async Task<KMLDataType> GetKMLDataType(int id)
        {
            return await this.Context.CompleteKMLDataTypes().FirstOrDefaultAsync(k => k.Id == id);
        }

        public async Task<bool> UpdateKMLDataType(KMLDataType kmlDataType)
        {
            var success = true;

            this.Context.Entry(kmlDataType).State = EntityState.Modified;

            try
            {
                await this.Context.SaveChangesAsync();
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

            return success;
        }

        public async void DeleteKMLDataType(KMLDataType kmlDataType)
        {
            this.Context.KMLDataTypes.Remove(kmlDataType);
            await this.Context.SaveChangesAsync();
        }

        public async Task<KMLDataType> FindKMLDataType(string name)
        {
            return await this.Context.CompleteKMLDataTypes().Where(e => e.Name == name).FirstOrDefaultAsync();
        }

        public bool KMLDataTypeExists(string name)
        {
            return this.Context.KMLDataTypes.Count(e => e.Name == name) > 0;
        }
    }
}
