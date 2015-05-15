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
using WeatherAPIModels.Models;

namespace CAPSWeatherAPI.Controllers
{
    public class FileTypesController : ApiController
    {
        private WeatherAPIContext Context = new WeatherAPIContext();

        // GET: api/FileTypes
        public IQueryable<FileType> GetFileTypes()
        {
            return this.Context.FileTypeService.GetAllFileTypes();
        }

        // GET: api/FileTypes
        [ResponseType(typeof(FileType))]
        public async Task<IHttpActionResult> GetFileType(int id)
        {
            var fileType = await this.Context.FileTypeService.GetFileType(id);

            if (fileType == null)
            {
                return NotFound();
            }

            return Ok(fileType);
        }

        // PUT: api/FileTypes
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFileType(int id, FileType fileType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fileType.Id)
            {
                return BadRequest();
            }

            var success = await this.Context.FileTypeService.UpdateFileType(fileType);

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

            fileType = await this.Context.FileTypeService.AddFileType(fileType);

            return CreatedAtRoute("DefaultApi", new { id = fileType.Id }, fileType);
        }

        // DELETE: api/FileTypes
        [ResponseType(typeof(FileType))]
        public async Task<IHttpActionResult> DeleteFileType(int id)
        {
            var fileType = await this.Context.FileTypeService.GetFileType(id);

            if (fileType == null)
            {
                return NotFound();
            }

            this.Context.FileTypeService.DeleteFileType(fileType);
            
            return Ok(fileType);
        }

    }
}