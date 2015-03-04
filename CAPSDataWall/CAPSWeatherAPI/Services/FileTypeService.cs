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
    public static class FileTypeService
    {
        public static async Task<FileType> AddFileType(FileType fileType)
        {
            FileType newFileType;

            using (var context = new WeatherAPIContext())
            {
                context.FileTypes.Add(fileType);
                await context.SaveChangesAsync();
                newFileType = await context.CompleteFileTypes().FirstOrDefaultAsync(f => f.ID == fileType.ID);
            }

            return newFileType;
        }

        public static IQueryable<FileType>  GetAllFileTypes()
        {
            IQueryable<FileType> allFileTypes;

            using (var context = new WeatherAPIContext())
            {
                allFileTypes = context.CompleteFileTypes();
            }

            return allFileTypes;
        }

        public static async Task<FileType> GetFileType(int id)
        {
            FileType fileType;

            using (var context = new WeatherAPIContext())
            {
                fileType = await context.CompleteFileTypes().FirstOrDefaultAsync(f => f.ID == id);
            }

            return fileType;

        }

        public static async Task<FileType> FindFileType(string name)
        {
            FileType fileType;

            using (var context = new WeatherAPIContext())
            {
                fileType = await context.CompleteFileTypes().Where(e => e.Name == name).FirstOrDefaultAsync();
            }

            return fileType;
        }

        public static async Task<bool> UpdateFileType(FileType fileType)
        {
            var success = true;

            using (var context = new WeatherAPIContext())
            {
                context.Entry(fileType).State = EntityState.Modified;

                try
                {
                    await context.SaveChangesAsync();
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
            }

            return success; 
        }

        public static async void DeleteFileType(FileType fileType)
        {
            using (var context = new WeatherAPIContext())
            {
                context.FileTypes.Remove(fileType);
                await context.SaveChangesAsync();
            }
        }

        public static bool FileTypeExists(string name)
        {
            bool exists;

            using (var context = new WeatherAPIContext())
            {
                exists = context.FileTypes.Count(e => e.Name == name) > 0;
            }

            return exists;
        }

    }
}
