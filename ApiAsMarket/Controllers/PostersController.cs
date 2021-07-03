using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiAsMarket.Models;
using ApiAsMarket.ViewModels;

namespace ApiAsMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostersController : ControllerBase
    {
        private readonly ApiAsMarketContext _context;

        public PostersController(ApiAsMarketContext context)
        {
            _context = context;
        }

        // GET: api/Posters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Poster>>> GetPoster()
        {
            return await _context.Poster.ToListAsync();
        }

        // GET: api/Posters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Poster>> GetPoster(int id)
        {
            var poster = await _context.Poster.FindAsync(id);

            if (poster == null)
            {
                return NotFound();
            }

            return poster;
        }

        // PUT: api/Posters/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPoster(int id, PosterViewModel poster)
        {
            if (id != poster.Id)
            {
                return BadRequest();
            }
            var PosterObj = _context.Poster.Find(id);
            PosterObj.Image1 = poster.Image1;
            PosterObj.Image2 = poster.Image2;
            PosterObj.IsDeleted = poster.IsDeleted;
            PosterObj.Name = poster.Name;
            PosterObj.ProductId = poster.ProductId;
            PosterObj.Caption = poster.Caption;
            _context.Entry(PosterObj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PosterExists(id))
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

        // POST: api/Posters
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Poster>> PostPoster(PosterViewModel poster)
        {
            var entity = _context.Poster.Add(new Poster
            {
                Image1 = poster.Image1,
                Image2 = poster.Image2,
                IsDeleted = poster.IsDeleted,
                Name = poster.Name,
                ProductId = poster.ProductId,
                Caption=poster.Caption
            });
            await _context.SaveChangesAsync();
            poster.Id = entity.Entity.Id;
            return CreatedAtAction("GetPoster", new { id = poster.Id }, poster);
        }

        // DELETE: api/Posters/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Poster>> DeletePoster(int id)
        {
            var poster = await _context.Poster.FindAsync(id);
            if (poster == null)
            {
                return NotFound();
            }

            _context.Poster.Remove(poster);
            await _context.SaveChangesAsync();

            return poster;
        }

        private bool PosterExists(int id)
        {
            return _context.Poster.Any(e => e.Id == id);
        }
    }
}
