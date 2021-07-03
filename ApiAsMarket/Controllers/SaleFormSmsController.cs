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
    public class SaleFormSmsController : ControllerBase
    {
        private readonly ApiAsMarketContext _context;

        public SaleFormSmsController(ApiAsMarketContext context)
        {
            _context = context;
        }

        // GET: api/SaleFormSms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleFormSms>>> GetSaleFormSms()
        {
            return await _context.SaleFormSms.ToListAsync();
        }

        // GET: api/SaleFormSms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleFormSms>> GetSaleFormSms(int id)
        {
            var saleFormSms = await _context.SaleFormSms.FindAsync(id);

            if (saleFormSms == null)
            {
                return NotFound();
            }

            return saleFormSms;
        }

        // PUT: api/SaleFormSms/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSaleFormSms(int id, SaleFormSmsViewModel saleFormSms)
        {
            if (id != saleFormSms.Id)
            {
                return BadRequest();
            }
            var saleFormSmsObj = await _context.SaleFormSms.FindAsync(id);
            saleFormSmsObj.OperatorId = saleFormSms.OperatorId;
            saleFormSmsObj.ProductId = saleFormSms.ProductId;
            saleFormSmsObj.CustomerMobile = saleFormSms.CustomerMobile;
            saleFormSmsObj.State = saleFormSms.State;
            saleFormSmsObj.SellerId = saleFormSms.SellerId;


            _context.Entry(saleFormSmsObj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleFormSmsExists(id))
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

        // POST: api/SaleFormSms
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SaleFormSms>> PostSaleFormSms(SaleFormSmsViewModel saleFormSms)
        {
            _context.SaleFormSms.Add(new SaleFormSms
            {
                CustomerMobile= saleFormSms.CustomerMobile,
                State= saleFormSms.State,
                ProductId= saleFormSms.ProductId,
                OperatorId= saleFormSms.OperatorId,
                SellerId= saleFormSms.SellerId

            });
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSaleFormSms", new { id = saleFormSms.Id }, saleFormSms);
        }

        // DELETE: api/SaleFormSms/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SaleFormSms>> DeleteSaleFormSms(int id)
        {
            var saleFormSms = await _context.SaleFormSms.FindAsync(id);
            if (saleFormSms == null)
            {
                return NotFound();
            }

            _context.SaleFormSms.Remove(saleFormSms);
            await _context.SaveChangesAsync();

            return saleFormSms;
        }

        private bool SaleFormSmsExists(int id)
        {
            return _context.SaleFormSms.Any(e => e.Id == id);
        }
    }
}
