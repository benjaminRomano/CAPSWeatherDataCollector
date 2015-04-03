using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
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
    public class KMLDataController : ApiController
    {

        private WeatherAPIContext Context = new WeatherAPIContext();

        //Expects date in date.toUTCString() format
        //returns kmlData at time 
        [Route("api/KMLData/")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDataAtTime(string dateString,string typeName)
        {

            var targetDate = DateTime.Parse(dateString);

            var kmlData = await this.Context.KMLDataService.GetDataAtTime(targetDate, typeName);

            if (kmlData == null)
            {
                return NotFound();
            }

            return Ok(kmlData);
        }

        [Route("api/KMLData")]
        [HttpGet]
        public IQueryable<KMLData> GetKMLData(string typeName)
        {
            return this.Context.KMLDataService.GetAllKMLData(typeName);
        }

        // GET: api/KMLData
        public IQueryable<KMLData> GetKMLData()
        {
            return this.Context.KMLDataService.GetAllKMLData();
        }

        // GET: api/KMLData/5
        [ResponseType(typeof(KMLData))]
        [Route("api/KMLData/")]
        [HttpGet]
        public async Task<IHttpActionResult> GetKMLData(int id)
        {
            var kmlData = await this.Context.KMLDataService.GetKMLData(id);

            if (kmlData == null)
            {
                return NotFound();
            }

            return Ok(kmlData);
        }

        // PUT: api/KMLData/5
        [ResponseType(typeof(void))]
        [Route("api/KMLData/")]
        [HttpPut]
        public async Task<IHttpActionResult> PutKMLData(int id, KMLData kmlData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != kmlData.ID)
            {
                return BadRequest();
            }

            var success = await this.Context.KMLDataService.UpdateKMLData(kmlData);

            if (!success)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/KMLData
        [ResponseType(typeof(KMLData))]
        [Route("api/KMLData/")]
        public async Task<IHttpActionResult> PostKMLData(KMLData kmlData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Replace types with their IDs if found
            if (this.Context.KMLDataTypeService.KMLDataTypeExists(kmlData.DataType.Name))
            {
                var foundDataType = await this.Context.KMLDataTypeService.FindKMLDataType(kmlData.DataType.Name);
                kmlData.DataTypeID = foundDataType.ID;
                kmlData.DataType = null;
            }
            else if(this.Context.FileTypeService.FileTypeExists(kmlData.DataType.FileType.Name))
            {
                var foundFileType = await this.Context.FileTypeService.FindFileType(kmlData.DataType.FileType.Name);
                kmlData.DataType.FileTypeID = foundFileType.ID;
                kmlData.DataType.FileType = null;
            }

            kmlData = await this.Context.KMLDataService.AddKMLData(kmlData);

            return Ok(kmlData);
        }

        // DELETE: api/KMLData/5
        [ResponseType(typeof(KMLData))]
        [Route("api/KMLData/")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteKMLData(int id)
        {
            var kmlData = await this.Context.KMLDataService.GetKMLData(id);

            if (kmlData == null)
            {
                return NotFound();
            }

            this.Context.KMLDataService.DeleteKMLData(kmlData); 

            return Ok(kmlData);
        }
    }
}