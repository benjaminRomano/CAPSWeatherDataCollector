using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CAPSWeatherAPI.Contexts;
using WeatherAPIModels.Models;

namespace CAPSWeatherAPI.Controllers
{
    public class KMLDataTypeController : ApiController
    {
        private WeatherAPIContext Context = new WeatherAPIContext();

        public IQueryable<KMLDataType> GetKMLDataTypes()
        {
            return this.Context.KMLDataTypeService.GetAllKMLDataTypes();
        }

        public async Task<IHttpActionResult> GetKMLDataType(int id)
        {
            var kmlDataType = await this.Context.KMLDataTypeService.GetKMLDataType(id);

            if (kmlDataType == null)
            {
                return NotFound();
            }

            return Ok(kmlDataType);
        }

        public async Task<IHttpActionResult> PutKMLDataType(int id, KMLDataType kmlDataType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != kmlDataType.Id)
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

        public async Task<IHttpActionResult> PostKMLDataType(KMLDataType kmlDataType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (this.Context.FileTypeService.FileTypeExists(kmlDataType.FileType.Name))
            {
                var foundFileType = await this.Context.FileTypeService.FindFileType(kmlDataType.FileType.Name);
                kmlDataType.FileTypeId = foundFileType.Id;
                kmlDataType.FileType = null;
            }

            kmlDataType = await this.Context.KMLDataTypeService.AddKMLDataType(kmlDataType);

            return CreatedAtRoute("DefaultApi", new { id = kmlDataType.Id }, kmlDataType);
        }

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