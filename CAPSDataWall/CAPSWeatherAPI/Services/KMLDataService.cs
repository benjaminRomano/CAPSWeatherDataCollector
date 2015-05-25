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
    public class KMLDataService
    {
        private WeatherDataContext Context { get; set; }

        public KMLDataService(WeatherDataContext weatherAPIContext)
        {
            this.Context = weatherAPIContext;
        }

        public async Task<KMLData> GetDataAtTime(DateTime targetDate, string typeName)
        {
            var nearestDataTimeDiff = await this.Context.KMLData.Where(k => k.DataType.Name == typeName).MinAsync(k => SqlFunctions.DateDiff("minute", k.CreatedAt, targetDate));

            if (nearestDataTimeDiff == null)
            {
                return null;
            }

            var nearestDiff = nearestDataTimeDiff.GetValueOrDefault();

            KMLData kmlData = null;

            //Only care if diff is less than or equal to 10 minutes
            if (Math.Abs(nearestDiff) <= 10)
            {
                kmlData = await this.Context.CompleteKMLData().Where(k => k.DataType.Name == typeName)
                    .FirstAsync(k => SqlFunctions.DateDiff("minute", k.CreatedAt, targetDate) == nearestDiff);
            }

            return kmlData;
        }

        public async Task<KMLData> GetNextRecord(KMLData currentKMLData)
        {
            return await this.Context.CompleteKMLData().Where(k => k.Id > currentKMLData.Id && k.DataTypeId == currentKMLData.DataTypeId)
                .OrderBy(k => k.Id).FirstOrDefaultAsync();
        }

        public async Task<KMLData> GetLatest(string kmlDataTypeName)
        {
            return await this.Context.CompleteKMLData().Where(k => k.DataType.Name == kmlDataTypeName).OrderByDescending(k => k.CreatedAt).FirstOrDefaultAsync();
        }

        public async Task<KMLData> AddKMLData(KMLData kmlData)
        {
            this.Context.KMLData.Add(kmlData);
            await this.Context.SaveChangesAsync();
            return await this.Context.CompleteKMLData().FirstOrDefaultAsync(k => k.Id == kmlData.Id);
        }

        public IQueryable<KMLData> GetAllKMLData()
        {
            return this.Context.CompleteKMLData();
        }

        public async Task<KMLData> GetKMLData(int id)
        {
            return await this.Context.CompleteKMLData().FirstOrDefaultAsync(k => k.Id == id);
        }

        public async Task<bool> UpdateKMLData(KMLData kmlData)
        {
            var success = true;

            this.Context.Entry(kmlData).State = EntityState.Modified;

            try
            {
                await this.Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KMLDataExists(kmlData.Id))
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

        public async void DeleteKMLData(KMLData kmlData)
        {
            this.Context.KMLData.Remove(kmlData);
            await this.Context.SaveChangesAsync();
        }

        public bool KMLDataExists(int id)
        {
            return this.Context.KMLData.Count(k => k.Id == id) > 0;
        }

        public IQueryable<KMLData> GetAllKMLData(string typeName)
        {
            return this.Context.KMLData.Where(k => k.DataType.Name == typeName);
        }
    }
}
