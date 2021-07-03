using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiAsMarket.Models;
using ApiAsMarket.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ApiAsMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ApiAsMarketContext _context;

        public FilesController(ApiAsMarketContext context)
        {
            _context = context;
        }

        // GET: api/Files
        [HttpGet]
        public async Task<ActionResult<IEnumerable<File>>> GetFile()
        {
            return await _context.File.ToListAsync();
        }

        // GET: api/Files/5
        [HttpGet("{id}")]
        public async Task<ActionResult<File>> GetFile(int id)
        {
            var file = await _context.File.FindAsync(id);

            if (file == null)
            {
                return NotFound();
            }

            return file;
        }

        // PUT: api/Files/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("UpdateFile")]
        public async Task<IActionResult> PutFile(File file)
        {

            var FileObj = _context.File.Find(file.Id);
            FileObj.IsDeleted = file.IsDeleted;
            FileObj.Link = file.Link;
            FileObj.Name = file.Name;
            FileObj.Date = file.Date;
            FileObj.Image = file.Image;
            FileObj.Description = file.Description;
            _context.Entry(FileObj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileExists(file.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Files
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<File>> PostFile(FileViewModel file)
        {
            var entity = _context.File.Add(new File
            {
                IsDeleted = file.IsDeleted,
                Link = file.Link,
                Name = file.Name,
                Date = file.Date,
                Image = file.Image,
                Description = file.Description
            });
            await _context.SaveChangesAsync();
            file.Id = entity.Entity.Id;
            return CreatedAtAction("GetFile", new { id = file.Id }, file);
        }

        // DELETE: api/Files/5
        [HttpPost("Delete/{id}")]
        public async Task<ActionResult<File>> DeleteFile(int id)
        {
            var file = await _context.File.FindAsync(id);
            if (file == null)
            {
                return NotFound();
            }

            _context.File.Remove(file);
            await _context.SaveChangesAsync();

            return file;
        }

        private bool FileExists(int id)
        {
            return _context.File.Any(e => e.Id == id);
        }

     
        private async Task<string> PostAttachment(byte[] data, Uri url, string contentType)
        {
            HttpContent content = new ByteArrayContent(data);

            content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            using (var form = new MultipartFormDataContent())
            {
                form.Add(content);

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(url, form);
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

    }
}
