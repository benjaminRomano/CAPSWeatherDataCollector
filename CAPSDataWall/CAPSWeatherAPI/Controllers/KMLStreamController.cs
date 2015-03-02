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
using WeatherAPIModels;
using WebGrease.Css.Ast.Selectors;

namespace CAPSWeatherAPI.Controllers
{
    public class KMLStreamController : ApiController
    {
        private WeatherAPIContext db = new WeatherAPIContext();

        // GET: api/KMLStream
        [Route("api/KMLStream")]
        [HttpGet]
        public IQueryable<KMLStream> GetKMLStreams()
        {
            return db.KMLStreams;
        }

        //GET "api/KMLStream"
        [ResponseType(typeof(KMLStream))]
        [Route("api/KMLStream")]
        [HttpGet]
        public async Task<IHttpActionResult> GetKMLStream(int id)
        {
            var kmlStream = await db.KMLStreams.FindAsync(id);
            if (kmlStream == null)
            {
                return NotFound();
            }

            return Ok(kmlStream);
        }

        //GET "api/KMLStream"
        [Route("api/KMLStream")]
        [HttpGet]
        [ResponseType(typeof(KMLStream))]
        public async Task<IHttpActionResult> GetKMLStream(KMLDataSource source, KMLDataType type, string name)
        {
            KMLStream kmlStream = await GetSpecificStream(source,type,name).FirstOrDefaultAsync();
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
        public async Task<IHttpActionResult> IncrementKMLStream(KMLDataSource source, KMLDataType type,string name)
        {
            var currentStatus = GetSpecificStream(source,type,name).FirstOrDefault();

            KMLData kmlData;
            //no record exists. Create record and set it to latest Data
            if (currentStatus == null)
            {

                kmlData = db.KMLData.Where(k => k.Type == type).OrderByDescending(k => k.CreatedAt).FirstOrDefault();

                //No data exists
                if (kmlData == null)
                {
                    return BadRequest("No Data");
                }

                var newKMLStream = new KMLStream()
                {
                    Type = kmlData.Type,
                    Source = source,
                    KMLData = kmlData,
                    Name = name,
                    Updated = true

                };

                db.KMLStreams.Add(newKMLStream);
                await db.SaveChangesAsync();
                return Ok(newKMLStream);

            }

            //Get next record
            kmlData = db.KMLData.Where(k => k.ID > currentStatus.KMLData.ID).OrderBy(k => k.ID).FirstOrDefault();

            //No new data return current
            if(kmlData == null)
            {
                return Ok(currentStatus);
            }

            currentStatus.KMLData = kmlData;
            currentStatus.Updated = true;


            db.Entry(currentStatus).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Ok(currentStatus);
        }

        // PUT: api/KMLStream/update
        [Route("api/KMLStream/update")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateKMLStream(KMLDataSource source, KMLData kmlData, string name)
        {
            if (!await db.KMLData.ContainsAsync(kmlData))
            {
                return BadRequest("KML Data not found");
            }

            var currentStatus = await GetSpecificStream(source,kmlData.Type,name).FirstOrDefaultAsync();

            //no data currently there create record
            if (currentStatus == null)
            {
                var newKMLStream = new KMLStream()
                {
                    Type = kmlData.Type,
                    Source = source,
                    KMLData = kmlData,
                    Name = name,
                    Updated = true

                };

                db.KMLStreams.Add(newKMLStream);
                await db.SaveChangesAsync();

                return Ok(newKMLStream);
            }

            //Record found update it
            currentStatus.KMLData = kmlData;
            currentStatus.Updated = true;

            db.Entry(currentStatus).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Ok(currentStatus);
        }

        // PUT: api/KMLStream/update
        [Route("api/KMLStream/update")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateKMLStream(KMLDataSource source,string name,int id)
        {
            var kmlData = await db.KMLData.FindAsync(id);
            if (kmlData == null)
            {
                return BadRequest("KML Data not found");
            }

            var currentStatus = await GetSpecificStream(source, kmlData.Type, name).FirstOrDefaultAsync();

            //no data currently there create record
            if (currentStatus == null)
            {
                var newKMLStream = new KMLStream()
                {
                    Type = kmlData.Type,
                    Source = source,
                    KMLData = kmlData,
                    Name = name,
                    Updated = true
                };

                db.KMLStreams.Add(newKMLStream);
                await db.SaveChangesAsync();

                return Ok(newKMLStream);
            }

            //Record found update it
            currentStatus.KMLData = kmlData;
            currentStatus.Updated = true;

            db.Entry(currentStatus).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Ok(currentStatus);
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

            db.Entry(kmlStream).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KMLStreamExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
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

            kmlStream.Updated = true;
            db.KMLStreams.Add(kmlStream);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = kmlStream.ID }, kmlStream);
        }

        // DELETE: api/KMLStream/5
        [ResponseType(typeof(KMLStream))]
        [Route("api/KMLStream")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteKMLStream(int id)
        {
            KMLStream kmlStream = await db.KMLStreams.FindAsync(id);
            if (kmlStream == null)
            {
                return NotFound();
            }

            db.KMLStreams.Remove(kmlStream);
            await db.SaveChangesAsync();

            return Ok(kmlStream);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KMLStreamExists(int id)
        {
            return db.KMLStreams.Count(e => e.ID == id) > 0;
        }

        private IQueryable<KMLStream> GetSpecificStream(KMLDataSource source, KMLDataType type, string name)
        {
            return db.KMLStreams.Where(c => c.Source == source && c.Type == type && c.Name == name);
        }

    }
}