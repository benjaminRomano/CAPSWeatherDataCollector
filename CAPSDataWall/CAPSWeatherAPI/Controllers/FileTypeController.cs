using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CAPSWeatherAPI.Contexts;
using WeatherAPIModels.Models;

namespace CAPSWeatherAPI.Controllers
{
    public class FileTypeController : ApiController
    {
        private WeatherAPIContext Context = new WeatherAPIContext();

        // GET: api/FileTypes
        [HttpGet]
        public IQueryable<FileType> GetFileTypes()
        {
            return this.Context.FileTypeService.GetAllFileTypes();
        }

        // GET: api/FileTypes
        [HttpGet]
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
        [HttpPut]
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
        [HttpPost]
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
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteFileType(int id)
        {
            var fileType = await this.Context.FileTypeService.GetFileType(id);

            if (fileType == null)
            {
                return NotFound();
            }

            this.Context.FileTypeService.DeleteFileType(fileType);
            
            return Ok();
        }

    }
}