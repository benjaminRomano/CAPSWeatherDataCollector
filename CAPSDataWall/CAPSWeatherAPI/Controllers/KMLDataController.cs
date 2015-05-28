using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CAPSWeatherAPI.Contexts;
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

        [Route("api/KMLData/")]
        [HttpGet]
        public IQueryable<KMLData> GetKMLData(string typeName)
        {
            return this.Context.KMLDataService.GetAllKMLData(typeName);
        }

        // GET: api/KMLData
        [Route("api/KMLData/")]
        [HttpGet]
        public IQueryable<KMLData> GetKMLData()
        {
            return this.Context.KMLDataService.GetAllKMLData();
        }

        [Route("api/KMLData/{id}")]
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

        [Route("api/KMLData/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> PutKMLData(int id, KMLData kmlData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != kmlData.Id)
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

        [Route("api/KMLData/")]
        [HttpPost]
        public async Task<IHttpActionResult> PostKMLData(KMLData kmlData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Replace kmlDataType and fileType with ids if they already exist
            var foundDataType = await this.Context.KMLDataTypeService.FindKMLDataType(kmlData.DataType.Name);

            if (foundDataType != null)
            {
                kmlData.DataTypeId = foundDataType.Id;
                kmlData.DataType = null;
            }
            else
            {
                var foundFileType = await this.Context.FileTypeService.FindFileType(kmlData.DataType.FileType.Name);
                if (foundFileType != null)
                {
                    kmlData.DataType.FileTypeId = foundFileType.Id;
                    kmlData.DataType.FileType = null;
                }
            }

            kmlData = await this.Context.KMLDataService.AddKMLData(kmlData);

            return Ok(kmlData);
        }

        [Route("api/KMLData/{id}")]
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