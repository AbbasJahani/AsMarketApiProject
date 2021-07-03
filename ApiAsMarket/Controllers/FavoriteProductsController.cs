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
    public class FavoriteProductsController : ControllerBase
    {
        private readonly ApiAsMarketContext _context;

        public FavoriteProductsController(ApiAsMarketContext context)
        {
            _context = context;
        }

        // GET: api/FavoriteProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FavoriteProduct>>> GetFavoriteProduct()
        {
            return await _context.FavoriteProduct.ToListAsync();
        }

        // GET: api/FavoriteProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FavoriteProduct>> GetFavoriteProduct(int id)
        {
            var favoriteProduct = await _context.FavoriteProduct.FindAsync(id);

            if (favoriteProduct == null)
            {
                return NotFound();
            }

            return favoriteProduct;
        }

        // PUT: api/FavoriteProducts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("Update")]
        public async Task<IActionResult> PutFavoriteProduct( FavoriteProduct favoriteProduct)
        {
         
            var FavoriteProductObj = _context.FavoriteProduct.Find(favoriteProduct.Id);
            FavoriteProductObj.IsDeleted = favoriteProduct.IsDeleted;
            FavoriteProductObj.ProductId = favoriteProduct.ProductId;
            FavoriteProductObj.UserId = favoriteProduct.UserId;
            _context.Entry(FavoriteProductObj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavoriteProductExists(favoriteProduct.Id))
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

        // POST: api/FavoriteProducts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<FavoriteProduct>> PostFavoriteProduct(FavoriteProductViewModel favoriteProduct)
        {
            var entity = _context.FavoriteProduct.Add(new FavoriteProduct
            {
                IsDeleted = favoriteProduct.IsDeleted,
                ProductId = favoriteProduct.ProductId,
                UserId = favoriteProduct.UserId


            });
            await _context.SaveChangesAsync();
            favoriteProduct.Id = entity.Entity.Id;
            return CreatedAtAction("GetFavoriteProduct", new { id = favoriteProduct.Id }, favoriteProduct);
        }

        // DELETE: api/FavoriteProducts/5
        [HttpPost("Delete/{id}")]
        public async Task<ActionResult<FavoriteProduct>> DeleteFavoriteProduct(int id)
        {
            var favoriteProduct = await _context.FavoriteProduct.FindAsync(id);
            if (favoriteProduct == null)
            {
                return NotFound();
            }

            _context.FavoriteProduct.Remove(favoriteProduct);
            await _context.SaveChangesAsync();

            return favoriteProduct;
        }

        private bool FavoriteProductExists(int id)
        {
            return _context.FavoriteProduct.Any(e => e.Id == id);
        }
    }
}
