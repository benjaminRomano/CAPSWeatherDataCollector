using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CAPSWeatherAPI.Contexts;
using CAPSWeatherAPI.Services;
using WeatherAPIModels;
using WeatherAPIModels.Models;

namespace CAPSWeatherAPI.Controllers
{
    public class FileTypesController : ApiController
    {

        // GET: api/FileTypes
        public IQueryable<FileType> GetFileTypes()
        {
            return FileTypeService.GetAllFileTypes();
        }

        // GET: api/FileTypes/5
        [ResponseType(typeof(FileType))]
        public async Task<IHttpActionResult> GetFileType(int id)
        {
            FileType fileType = await FileTypeService.GetFileType(id);

            if (fileType == null)
            {
                return NotFound();
            }

            return Ok(fileType);
        }

        // PUT: api/FileTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFileType(int id, FileType fileType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fileType.ID)
            {
                return BadRequest();
            }

            var success = await FileTypeService.UpdateFileType(fileType);

            if (!success)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/FileTypes
        [ResponseType(typeof(FileType))]
        public async Task<IHttpActionResult> PostFileType(FileType fileType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            fileType = await FileTypeService.AddFileType(fileType);

            return CreatedAtRoute("DefaultApi", new { id = fileType.ID }, fileType);
        }

        // DELETE: api/FileTypes/5
        [ResponseType(typeof(FileType))]
        public async Task<IHttpActionResult> DeleteFileType(int id)
        {
            FileType fileType = await FileTypeService.GetFileType(id);
            if (fileType == null)
            {
                return NotFound();
            }

            FileTypeService.DeleteFileType(fileType);
            
            return Ok(fileType);
        }

    }
}