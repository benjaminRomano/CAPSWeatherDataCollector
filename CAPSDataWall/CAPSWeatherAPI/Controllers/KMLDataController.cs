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
using WeatherAPIModels;

namespace CAPSWeatherAPI.Controllers
{
    public class KMLDataController : ApiController
    {
        private WeatherAPIContext db = new WeatherAPIContext();



        //Expects date in date.toUTCString() format
        //returns kmlData at time 
        [Route("api/KMLData/")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDataAtTime(string dateString,KMLDataType type)
        {

            var targetDate = DateTime.Parse(dateString);
            var nearestDiffNulllable = db.KMLData.Where(c=> c.Type == type).Min(c => SqlFunctions.DateDiff("minute", c.CreatedAt, targetDate));

            if (nearestDiffNulllable == null)
            {
                return NotFound();
            }

            var nearestDiff = nearestDiffNulllable.GetValueOrDefault();

            //Return null if no object in range
            if (Math.Abs(nearestDiff) > 30)
            {
                return NotFound();
            }

            var kmlData = await db.KMLData.Where(c=> c.Type == type).FirstAsync(c => SqlFunctions.DateDiff("minute", c.CreatedAt, targetDate) == nearestDiff);
            return Ok(kmlData);
        }

        // GET: api/KMLData
        public IQueryable<KMLData> GetKMLData()
        {
            return db.KMLData;
        }

        // GET: api/KMLData/5
        [ResponseType(typeof(KMLData))]
        [Route("api/KMLData/")]
        [HttpGet]
        public async Task<IHttpActionResult> GetKMLData(int id)
        {
            KMLData kmlData = await db.KMLData.FindAsync(id);
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

            db.Entry(kmlData).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KMLDataExists(id))
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

        // POST: api/KMLData
        [ResponseType(typeof(KMLData))]
        [Route("api/KMLData/")]
        public async Task<IHttpActionResult> PostKMLData(KMLData kmlData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.KMLData.Add(kmlData);
            await db.SaveChangesAsync();

            kmlData = await db.KMLData.OrderByDescending(c => c.ID).FirstAsync();

            return Ok(kmlData);
        }

        // DELETE: api/KMLData/5
        [ResponseType(typeof(KMLData))]
        [Route("api/KMLData/")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteKMLData(int id)
        {
            KMLData kmlData = await db.KMLData.FindAsync(id);
            if (kmlData == null)
            {
                return NotFound();
            }

            db.KMLData.Remove(kmlData);
            await db.SaveChangesAsync();

            return Ok(kmlData);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KMLDataExists(int id)
        {
            return db.KMLData.Count(e => e.ID == id) > 0;
        }
    }
}