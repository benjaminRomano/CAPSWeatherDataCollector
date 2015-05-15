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
    public class FileTypeService
    {
        private WeatherDataContext Context { get; set; }

        public FileTypeService(WeatherDataContext context)
        {
            this.Context = context;
        }

        public async Task<FileType> AddFileType(FileType fileType)
        {
            this.Context.FileTypes.Add(fileType);
            await this.Context.SaveChangesAsync();
            return await this.Context.CompleteFileTypes().FirstOrDefaultAsync(f => f.Id == fileType.Id);
        }

        public IQueryable<FileType>  GetAllFileTypes()
        {
            return this.Context.CompleteFileTypes();
        }

        public async Task<FileType> GetFileType(int id)
        {
            return await this.Context.CompleteFileTypes().FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<FileType> FindFileType(string name)
        {
           return await this.Context.CompleteFileTypes().Where(e => e.Name == name).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateFileType(FileType fileType)
        {
            var success = true;

            this.Context.Entry(fileType).State = EntityState.Modified;

            try
            {
                await this.Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileTypeExists(fileType.Name))
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

        public async void DeleteFileType(FileType fileType)
        {
            this.Context.FileTypes.Remove(fileType);
            await this.Context.SaveChangesAsync();
        }

        public bool FileTypeExists(string name)
        {
            return this.Context.FileTypes.Count(e => e.Name == name) > 0;
        }
    }
}