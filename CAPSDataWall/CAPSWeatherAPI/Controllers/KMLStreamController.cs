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
using System.Web.Http.Results;
using CAPSWeatherAPI.Contexts;
using CAPSWeatherAPI.Services;
using WeatherAPIModels;
using WeatherAPIModels.Models;
using WebGrease.Css.Ast.Selectors;

namespace CAPSWeatherAPI.Controllers
{
    public class KMLStreamController : ApiController
    {
        private WeatherAPIContext Context = new WeatherAPIContext();

        // GET: api/KMLStream
        [Route("api/KMLStream")]
        [HttpGet]
        public IQueryable<KMLStream> GetKMLStreams()
        {
            return this.Context.KMLStreamService.GetAllKMLStreams();
        }

        //GET: api/KMLStream
        [ResponseType(typeof(KMLStream))]
        [Route("api/KMLStream")]
        [HttpGet]
        public async Task<IHttpActionResult> GetKMLStream(int id)
        {
            var kmlStream = await this.Context.KMLStreamService.GetKMLStream(id);

            if (kmlStream == null)
            {
                return NotFound();
            }

            return Ok(kmlStream);
        }

        //GET: api/KMLStream
        [Route("api/KMLStream")]
        [HttpGet]
        [ResponseType(typeof(KMLStream))]
        public async Task<IHttpActionResult> GetKMLStream(KMLDataSource source, string typeName, string name)
        {
            var kmlStream = await this.Context.KMLStreamService.GetSpecificStream(source,typeName,name);

            if (kmlStream == null)
            {
                return NotFound();
            }

            return Ok(kmlStream);
        }


        //Increment KMLData to next available record
        // PUT: api/KMLStream/increment
        [Route("api/KMLStream/increment")]
        [HttpPut]
        public async Task<IHttpActionResult> IncrementKMLStream(KMLDataSource source, string typeName,string name)
        {
            var currentStream = await this.Context.KMLStreamService.GetSpecificStream(source, typeName, name);

            KMLData kmlData;
            //no record exists. Create record and set it to latest Data
            if (currentStream == null)
            {

                kmlData = await this.Context.KMLDataService.GetLatest(typeName);

                //No data exists
                if (kmlData == null)
                {
                    return BadRequest("No Data");
                }

                var newKMLStream = new KMLStream()
                {
                    Source = source,
                    KMLDataID = kmlData.ID,
                    Name = name,
                    Updated = true

                };

                newKMLStream = await this.Context.KMLStreamService.AddKMLStream(newKMLStream);
                return Ok(newKMLStream);
            }

            //Get next record
            kmlData = await this.Context.KMLDataService.GetNextRecord(currentStream.KMLData);

            //No new data return current record
            if(kmlData == null)
            {
                return Ok(currentStream);
            }

            currentStream.KMLDataID = kmlData.ID;
            currentStream.KMLData = kmlData;
            currentStream.Updated = true;

            var success =  await this.Context.KMLStreamService.UpdateKMLStream(currentStream);

            if (!success)
            {
                NotFound();
            }

            return Ok(currentStream);
        }

        // PUT: api/KMLStream/update
        [Route("api/KMLStream/update")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateKMLStream(KMLDataSource source, KMLData kmlData, string name)
        {
            var exists = this.Context.KMLDataService.KMLDataExists(kmlData.ID);

            if (!exists)
            {
                return BadRequest("KML Data not found");
            }

            var currentStream = await this.Context.KMLStreamService.GetSpecificStream(source,kmlData.DataType.Name,name);

            //no data currently there create record
            if (currentStream == null)
            {
                var newKMLStream = new KMLStream()
                {
                    Source = source,
                    KMLDataID = kmlData.ID,
                    Name = name,
                    Updated = true

                };

                newKMLStream = await this.Context.KMLStreamService.AddKMLStream(newKMLStream);

                return Ok(newKMLStream);
            }

            //Record found update it
            currentStream.KMLData = kmlData;
            currentStream.Updated = true;

            var success = await this.Context.KMLStreamService.UpdateKMLStream(currentStream);

            if (!success)
            {
                return NotFound();
            }

            return Ok(currentStream);
        }

        // PUT: api/KMLStream/update
        [Route("api/KMLStream/update")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateKMLStream(KMLDataSource source,string name,int kmlDataId)
        {
            var kmlData = await this.Context.KMLDataService.GetKMLData(kmlDataId);

            if (kmlData == null)
            {
                return BadRequest("KML Data not found");
            }

            var currentStream = await this.Context.KMLStreamService.GetSpecificStream(source, kmlData.DataType.Name, name);

            //no data currently there. Create record
            if (currentStream == null)
            {
                var newKMLStream = new KMLStream()
                {
                    Source = source,
                    Name = name,
                    Updated = true,
                    KMLDataID = kmlData.ID
                };

                newKMLStream = await this.Context.KMLStreamService.AddKMLStream(newKMLStream);

                return Ok(newKMLStream);
            }

            //Record found update it
            currentStream.KMLData = kmlData;
            currentStream.KMLDataID = kmlData.ID;
            currentStream.Updated = true;

            var success = await this.Context.KMLStreamService.UpdateKMLStream(currentStream);

            if (!success)
            {
                return NotFound();
            }

            return Ok(currentStream);
        }

        // PUT: api/KMLStream/5
        [ResponseType(typeof(void))]
        [Route("api/KMLStream/update")]
        [HttpPut]
        public async Task<IHttpActionResult> PutKMLStream(int id, KMLStream kmlStream)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != kmlStream.ID)
            {
                return BadRequest();
            }

            var success = await this.Context.KMLStreamService.UpdateKMLStream(kmlStream);

            if (!success)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/KMLStream
        [ResponseType(typeof(KMLStream))]
        [Route("api/KMLStream")]
        [HttpPost]
        public async Task<IHttpActionResult> PostKMLStream(KMLStream kmlStream)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var specificStream = await this.Context.KMLStreamService.GetSpecificStream(kmlStream.Source, kmlStream.KMLData.DataType.Name, kmlStream.Name);

            if (specificStream == null) 
            {
                return BadRequest("Data already exists");
            }

            kmlStream.Updated = true;

            kmlStream = await this.Context.KMLStreamService.AddKMLStream(kmlStream);

            return CreatedAtRoute("DefaultApi", new { id = kmlStream.ID }, kmlStream);
        }

        // DELETE: api/KMLStream/5
        [ResponseType(typeof(KMLStream))]
        [Route("api/KMLStream")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteKMLStream(int id)
        {
            var kmlStream = await this.Context.KMLStreamService.GetKMLStream(id);

            if (kmlStream == null)
            {
                return NotFound();
            }

            this.Context.KMLStreamService.DeleteKMLStream(kmlStream);

            return Ok(kmlStream);
        }
    }
}