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
    public class TiketReciversController : ControllerBase
    {
        private readonly ApiAsMarketContext _context;

        public TiketReciversController(ApiAsMarketContext context)
        {
            _context = context;
        }

        // GET: api/TiketRecivers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TiketReciver>>> GetTiketReciver()
        {
            return await _context.TiketReciver.ToListAsync();
        }

        // GET: api/TiketRecivers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TiketReciver>> GetTiketReciver(int id)
        {
            var tiketReciver = await _context.TiketReciver.FindAsync(id);

            if (tiketReciver == null)
            {
                return NotFound();
            }

            return tiketReciver;
        }

        // PUT: api/TiketRecivers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTiketReciver(int id, TiketReciver tiketReciver)
        {
            if (id != tiketReciver.Id)
            {
                return BadRequest();
            }
            var TiketReciverObj = _context.TiketReciver.Find(id);
            TiketReciverObj.IsDeleted = tiketReciver.IsDeleted;
            TiketReciverObj.ReciverId = tiketReciver.ReciverId;
            _context.Entry(TiketReciverObj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TiketReciverExists(id))
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

        // POST: api/TiketRecivers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TiketReciver>> PostTiketReciver(TiketReciverViewModel tiketReciver)
        {
            var entity = _context.TiketReciver.Add(new TiketReciver
            {
                IsDeleted = tiketReciver.IsDeleted,
                ReciverId = tiketReciver.ReciverId
            });
            await _context.SaveChangesAsync();
            tiketReciver.Id = entity.Entity.Id;
            return CreatedAtAction("GetTiketReciver", new { id = tiketReciver.Id }, tiketReciver);
        }

        // DELETE: api/TiketRecivers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TiketReciver>> DeleteTiketReciver(int id)
        {
            var tiketReciver = await _context.TiketReciver.FindAsync(id);
            if (tiketReciver == null)
            {
                return NotFound();
            }

            _context.TiketReciver.Remove(tiketReciver);
            await _context.SaveChangesAsync();

            return tiketReciver;
        }

        private bool TiketReciverExists(int id)
        {
            return _context.TiketReciver.Any(e => e.Id == id);
        }
    }
}
