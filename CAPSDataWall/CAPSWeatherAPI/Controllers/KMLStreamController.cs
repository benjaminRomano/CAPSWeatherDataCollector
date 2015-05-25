using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CAPSWeatherAPI.Contexts;
using WeatherAPIModels.Models;
using WeatherAPIModels.Utilities;

namespace CAPSWeatherAPI.Controllers
{
    public class KMLStreamController : ApiController
    {
        public IHttpActionResult Options()
        {
            return Ok();
        }

        private WeatherAPIContext Context = new WeatherAPIContext();

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

        [Route("api/KMLStream/DataTypes")]
        [HttpGet]
        public IEnumerable<String> GetDataTypes(string name)
        {
            return this.Context.KMLStreamService.GetDataTypes(name);
        }

        //GET: api/KMLStream
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

        [Route("api/KMLStream")]
        [HttpGet]
        public async Task<IHttpActionResult> GetKMLStream(string kmlDataTypeName, string streamName)
        {
            var description = new StreamDescription(kmlDataTypeName,streamName);

            var kmlStream = await this.Context.KMLStreamService.GetKMLStream(description);

            if (kmlStream == null)
            {
                return NotFound();
            }

            return Ok(kmlStream);
        }


        [Route("api/KMLStream/Increment")]
        [HttpPut]
        public async Task<IHttpActionResult> IncrementKMLStream(string kmlDataTypeName, string streamName)
        {
            var description = new StreamDescription(kmlDataTypeName, streamName);

            var stream = await this.Context.KMLStreamService.GetKMLStream(description);

            KMLData kmlData;
            //no record exists. Create record and set it to latest Data
            if (stream == null)
            {
                kmlData = await this.Context.KMLDataService.GetLatest(description.KMLDataTypeName);

                //No data exists
                if (kmlData == null)
                {
                    return BadRequest("No Data");
                }

                var newKMLStream = new KMLStream()
                {
                    KMLDataId = kmlData.Id,
                    Name = description.StreamName,
                };

                newKMLStream = await this.Context.KMLStreamService.AddKMLStream(newKMLStream);
                return Ok(newKMLStream);
            }

            kmlData = await this.Context.KMLDataService.GetNextRecord(stream.KMLData);

            //No new data return current record
            if(kmlData == null)
            {
                return Ok(stream);
            }

            stream.KMLDataId = kmlData.Id;
            stream.KMLData = kmlData;

            var success =  await this.Context.KMLStreamService.UpdateKMLStream(stream);

            if (!success)
            {
                NotFound();
            }

            return Ok(stream);
        }

        [Route("api/KMLStream")]
        [HttpPut]
        public async Task<IHttpActionResult> PutKMLStream(int kmlDataId, string kmlDataTypeName, string streamName)
        {

            var description = new StreamDescription(kmlDataTypeName, streamName);

            var kmlData = await this.Context.KMLDataService.GetKMLData(kmlDataId);

            if (kmlData == null)
            {
                return BadRequest("KML Data not found");
            }

            var stream = await this.Context.KMLStreamService.GetKMLStream(description);

            //no data currently there. Create record
            if (stream == null)
            {
                var newKMLStream = new KMLStream()
                {
                    Name = description.StreamName,
                    KMLDataId = kmlData.Id
                };

                newKMLStream = await this.Context.KMLStreamService.AddKMLStream(newKMLStream);

                return Ok(newKMLStream);
            }

            //Record found update it
            stream.KMLData = kmlData;
            stream.KMLDataId = kmlData.Id;

            var success = await this.Context.KMLStreamService.UpdateKMLStream(stream);

            if (!success)
            {
                return NotFound();
            }

            return Ok(stream);
        }

        // PUT: api/KMLStream
        [Route("api/KMLStream")]
        [HttpPut]
        public async Task<IHttpActionResult> PutKMLStream(int id, KMLStream stream)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stream.Id)
            {
                return BadRequest();
            }

            var success = await this.Context.KMLStreamService.UpdateKMLStream(stream);

            if (!success)
            {
                return NotFound();
            }

            return Ok(stream);
        }

        // POST: api/KMLStream
        [Route("api/KMLStream")]
        [HttpPost]
        public async Task<IHttpActionResult> PostKMLStream(KMLStream stream)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var description = new StreamDescription(stream.KMLData.DataType.Name, stream.Name);

            var existingStream = await this.Context.KMLStreamService.GetKMLStream(description);

            if (existingStream != null) 
            {
                return BadRequest("Data already exists");
            }

            stream = await this.Context.KMLStreamService.AddKMLStream(stream);

            return CreatedAtRoute("DefaultApi", new { id = stream.Id }, stream);
        }

        // DELETE: api/KMLStream
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