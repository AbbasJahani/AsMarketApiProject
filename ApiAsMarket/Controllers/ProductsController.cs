using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiAsMarket.Models;
using ApiAsMarket.ViewModels;
using Newtonsoft.Json;

namespace ApiAsMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApiAsMarketContext _context;

        public ProductsController(ApiAsMarketContext context)
        {
            _context = context;
        }

        // GET: api/Products
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        //{
        //    return await _context.Product.ToListAsync();
        //}



        [HttpGet("AllProducts")]
        public ActionResult<string> GetProduct()
        {
            var products = _context.Product.OrderByDescending(p => p.Id).ToList();

            if (products.Count < 1)
            {
                return NotFound();
            }

            var e = JsonConvert.SerializeObject(products, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return e;
        }

        [HttpGet("MinimumInventory")]
        public async Task<ActionResult<IEnumerable<Product>>> MinimumInventory()
        {
            return await _context.Product.OrderBy(x => x.Amount).ToListAsync();
        }

        [HttpGet("AllProductsCountByCatId/{categoryId}")]
        public ActionResult<int> GetProductsOfCategoryCount(int categoryId)
        {
            bool productCategory = _context.ProductCategory.Any(pc => pc.Id == categoryId);

            if (!productCategory)
            {
                return NotFound("دسته بندی کالا یافت نشد");
            }

            int productsCount = _context.Product.Where(p => p.CategoryId == categoryId).Count();

            return productsCount;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public ActionResult<string> GetProduct(int id)
        {
            var product =  _context.Product.Include(x=>x.Poster).FirstOrDefault(x=>x.Id==id);

            if (product == null)
            {
                return NotFound();
            }
            var e = JsonConvert.SerializeObject(product, Formatting.None,
                 new JsonSerializerSettings()
                 {
                     ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                 });
            return e;
        }

        [HttpGet("AllProductsCount")]
        public ActionResult<int> GetProductsCount()
        {
            int productsCount = _context.Product.Count();

            return productsCount;
        }

        [HttpGet("GetAProductByCategoryId/{CategoryId}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategoryId(int CategoryId)
        {
            var product = await _context.Product.Where(x => x.CategoryId == CategoryId).ToListAsync();

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpGet("GetProductByCode/{Code}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCode(String Code)
        {
            var product = await _context.Product.Where(x => x.Code == Code).ToListAsync();

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        //api/Products/ByPage/5
        [HttpGet("ByPage/{pgId}")]
        //[ResponseCache(Duration = 900, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<ActionResult<string>> GetProductByPageNum(int pgId)
        {
            int productsPerPage = 12;
            int skip = ((pgId - 1) * productsPerPage);

            var products = await _context.Product.OrderByDescending(p => p.Id).Skip(skip)
                .Take(productsPerPage).ToListAsync();

            if (products == null)
            {
                return NotFound();
            }

            var result = JsonConvert.SerializeObject(products, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return result;
        }

        // api/Products/GetProductOfCategoryByPage?catId=2&pgNum=1
        [HttpGet("GetProductOfCategoryByPage")]
        public async Task<ActionResult<string>> GetProductOfCategoryByPage(int catId, int pgNum)
        {
            int productsPerPage = 12;
            int skip = ((pgNum - 1) * productsPerPage);

            var products = await _context.Product.Where(p => p.CategoryId == catId).OrderByDescending(p => p.Id).Skip(skip)
                .Take(productsPerPage).ToListAsync();

            if (products == null)
            {
                return NotFound();
            }

            var result = JsonConvert.SerializeObject(products, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return result;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductViewModel product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            var ProductObj = _context.Product.Find(id);
            ProductObj.IsDeleted = product.IsDeleted;
            ProductObj.Name = product.Name;
            ProductObj.Image2 = product.Image2;
            ProductObj.Image1 = product.Image1;
            ProductObj.Image3 = product.Image3;
            ProductObj.Amount = product.Amount;
            ProductObj.Attribute = product.Attribute;
            ProductObj.CategoryId = product.CategoryId;
            ProductObj.Code = product.Code;
            ProductObj.Commission = product.Commission;
            ProductObj.Price = product.Price;
            ProductObj.PriceWithOff = product.PriceWithOff;
            ProductObj.Takhfif = product.Takhfif;

            _context.Entry(ProductObj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductViewModel product)
        {
           var entity= _context.Product.Add(new Product
            {
                IsDeleted = product.IsDeleted,
                Name= product.Name,
                Image2= product.Image2,
                Image1= product.Image1,
                Image3= product.Image3,
                Amount= product.Amount,
                Attribute= product.Attribute,
                CategoryId= product.CategoryId,
                Code= product.Code,
                Commission= product.Commission,
                Price= product.Price,
                PriceWithOff= product.PriceWithOff,
                Takhfif= product.Takhfif
               
            });
            await _context.SaveChangesAsync();
            product.Id = entity.Entity.Id;
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
