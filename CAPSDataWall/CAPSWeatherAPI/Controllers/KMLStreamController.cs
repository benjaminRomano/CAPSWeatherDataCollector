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
        public IHttpActionResult Options()
        {
            return Ok();
        }

        private WeatherAPIContext Context = new WeatherAPIContext();

        // GET: api/KMLStream
        [Route("api/KMLStream")]
        [HttpGet]
        public IQueryable<KMLStream> GetKMLStreams()
        {
            return this.Context.KMLStreamService.GetAllKMLStreams();
        }

        [Route("api/KMLStream/Names")]
        [HttpGet]
        public IEnumerable<String> GetStreamNames()
        {
            return this.Context.KMLStreamService.GetStreamNames();
        }

        [Route("api/KMLStream/Sources")]
        [HttpGet]
        public  IEnumerable<string> GetSources(string name)
        {
            var sources = this.Context.KMLStreamService.GetSources(name);
            return sources.Select(s => Enum.GetName(typeof (KMLDataSource), s)).Distinct();
        }

        [Route("api/KMLStream/DataTypes")]
        [HttpGet]
        public IEnumerable<String> GetDataTypes(string name, string source)
        {

            KMLDataSource kmlDataSource;
            var success = Enum.TryParse(source, true, out kmlDataSource);

            if (!success)
            {
                return Enumerable.Empty<String>();
            }

            return this.Context.KMLStreamService.GetDataTypes(name, kmlDataSource);
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
        public async Task<IHttpActionResult> IncrementKMLStream(KMLDataSource source, string typeName,string name, bool setUpdated)
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
                    Updated = setUpdated

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

            var success =  await this.Context.KMLStreamService.UpdateKMLStream(currentStream);

            if (!success)
            {
                NotFound();
            }

            return Ok(currentStream);
        }

        // PUT: api/KMLStream/update
        [HttpPut]
        [Route("api/KMLStream/update")]
        public async Task<IHttpActionResult> UpdateKMLStream(KMLDataSource source, KMLData kmlData, string name, bool setUpdated)
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
                    Updated = setUpdated

                };

                newKMLStream = await this.Context.KMLStreamService.AddKMLStream(newKMLStream);

                return Ok(newKMLStream);
            }

            //Record found update it
            currentStream.KMLData = kmlData;
            if (setUpdated)
            {
                currentStream.Updated = true;
            }

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
        public async Task<IHttpActionResult> UpdateKMLStream(KMLDataSource source,string name,int kmlDataId,bool setUpdated)
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
                    Updated = setUpdated,
                    KMLDataID = kmlData.ID
                };

                newKMLStream = await this.Context.KMLStreamService.AddKMLStream(newKMLStream);

                return Ok(newKMLStream);
            }

            //Record found update it
            currentStream.KMLData = kmlData;
            currentStream.KMLDataID = kmlData.ID;
            if (setUpdated)
            {
                currentStream.Updated = true;
            }

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