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
    public class KMLDataTypesController : ApiController
    {
        private WeatherAPIContext Context = new WeatherAPIContext();

        // GET: api/KMLDataTypes
        public IQueryable<KMLDataType> GetKMLDataTypes()
        {
            return this.Context.KMLDataTypeService.GetAllKMLDataTypes();
        }

        // GET: api/KMLDataTypes/5
        [ResponseType(typeof(KMLDataType))]
        public async Task<IHttpActionResult> GetKMLDataType(int id)
        {
            var kmlDataType = await this.Context.KMLDataTypeService.GetKMLDataType(id);

            if (kmlDataType == null)
            {
                return NotFound();
            }

            return Ok(kmlDataType);
        }

        // PUT: api/KMLDataTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutKMLDataType(int id, KMLDataType kmlDataType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != kmlDataType.ID)
            {
                return BadRequest();
            }

            var success = await this.Context.KMLDataTypeService.UpdateKMLDataType(kmlDataType);

            if (!success)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/KMLDataTypes
        [ResponseType(typeof(KMLDataType))]
        public async Task<IHttpActionResult> PostKMLDataType(KMLDataType kmlDataType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (this.Context.FileTypeService.FileTypeExists(kmlDataType.FileType.Name))
            {
                var foundFileType = await this.Context.FileTypeService.FindFileType(kmlDataType.FileType.Name);
                kmlDataType.FileTypeID = foundFileType.ID;
                kmlDataType.FileType = null;
            }

            kmlDataType = await this.Context.KMLDataTypeService.AddKMLDataType(kmlDataType);

            return CreatedAtRoute("DefaultApi", new { id = kmlDataType.ID }, kmlDataType);
        }

        // DELETE: api/KMLDataTypes/5
        [ResponseType(typeof(KMLDataType))]
        public async Task<IHttpActionResult> DeleteKMLDataType(int id)
        {
            var kmlDataType = await this.Context.KMLDataTypeService.GetKMLDataType(id);

            if (kmlDataType == null)
            {
                return NotFound();
            }

            this.Context.KMLDataTypeService.DeleteKMLDataType(kmlDataType);

            return Ok(kmlDataType);
        }
    }
}