using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiAsMarket.Models;
using ApiAsMarket.Core;
using ApiAsMarket.ViewModels;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Razor.Language;
using Newtonsoft.Json;

namespace ApiAsMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ApiAsMarketContext _context;

        public SalesController(ApiAsMarketContext context)
        {
            _context = context;
        }

        [AuthorizeOprator]
        [HttpGet("GetAllSales")]
        public async Task<ActionResult<string>> GetSales()
        {
            var sales = await _context.Sale.OrderByDescending(s => s.Id).ToListAsync();

            if (sales.Count < 1)
            {
                return NotFound("فاکتوری موجود نیست");
            }
          
            var result = JsonConvert.SerializeObject(sales, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return result;
        }

        [AuthorizeOprator]
        [HttpGet("GetAllSales/{pgNum}")]
        public async Task<ActionResult<string>> GetSales(int pgNum)
        {
            int salesPerPage = 10;

            int skip = ((pgNum - 1) * salesPerPage);

            var sales = await _context.Sale.OrderByDescending(s => s.Id).Skip(skip).Take(salesPerPage).ToListAsync();

            if (sales.Count < 1)
            {
                return NotFound("فاکتوری وجود ندارد");
            }

            var result = JsonConvert.SerializeObject(sales, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return result;
        }

        [AuthorizeOprator]
        [HttpGet("GetFinalSale")]
        public async Task<ActionResult<string>> GetFinalSale()
        {
            var sales = await _context.Sale.Where(s=>s.IsFinally == true && s.State == 2).OrderByDescending(s => s.Id).ToListAsync();

            if (sales.Count < 1)
            {
                return NotFound("فاکتور تکمیل شده ای وجود ندارد");
            }

            var result = JsonConvert.SerializeObject(sales, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return result;
        }

        // GET: api/Sales/5
        [AuthorizeOprator]
        [HttpGet("GetSale/{id}")]
        public async Task<ActionResult<string>> GetSale(int id)
        {
            var sale = await _context.Sale.Where(s => s.Id == id).SingleOrDefaultAsync();

            if (sale == null)
            {
                return NotFound("فاکتور مورد نظر یافت نشد");
            }

            var result = JsonConvert.SerializeObject(sale, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return result;
        }

        [AuthorizeOprator]
        [HttpGet("GetCurrentSaleByCustomer/{customerId}")]
        public IActionResult GetCurrentSaleByCustomer(int customerId)
        {
            if (!CustomerExist(customerId))
                return NotFound("مشتری پیدا نشد");

            var salesByCustomer = _context.Sale.Where(s => s.CustomerId == customerId && !s.IsFinally && s.State == 1).FirstOrDefault();

            if (salesByCustomer == null)
            {
                return NotFound("سبد خرید شما خالی است");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var saleDto = new SaleViewModel()
            {
                Id = salesByCustomer.Id,
                CustomerId = salesByCustomer.CustomerId,
                SellerId = salesByCustomer.SellerId,
                OperatorId = salesByCustomer.OperatorId,
                State = salesByCustomer.State,
                SaleDate = salesByCustomer.SaleDate,
                Commission = salesByCustomer.Commission,
                TotalPrice = salesByCustomer.TotalPrice,
                IsDeleted = salesByCustomer.IsDeleted,
                IsFinally = salesByCustomer.IsFinally,
                TrackingCode = salesByCustomer.TrackingCode
            };

            return Ok(saleDto);
        }

        [AuthorizeOprator]
        [HttpGet("GetSaleDetails/{saleId}")]
        public IActionResult GetSaleDetailsBySaleId(int saleId)
        {
            if (!SaleExist(saleId))
                return NotFound("فاکتوری با این آیدی یافت نشد");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var saleDetails = _context.SaleDetails.Where(s => s.SaleId == saleId).ToList();

            var result = JsonConvert.SerializeObject(saleDetails, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return Ok(result);
        }

        [AuthorizeOprator]
        [HttpGet("GetAllFinallySalesByPageNum/{pageNum}")]
        public IActionResult GetAllFinallySales(int pageNum)
        {
            int salesPerPage = 10;
            int skip = ((pageNum - 1) * salesPerPage);

            var sales = _context.Sale.Where(s => s.IsFinally && s.State == 2).OrderByDescending(s => s.SaleDate)
                .Skip(skip).Take(salesPerPage).ToList();

            if (sales.Count < 1)
            {
                return NotFound("فاکتور پرداخت شده ای یافت نشد");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = JsonConvert.SerializeObject(sales, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Ok(result);
        }

        [AuthorizeOprator]
        [HttpGet("FindWithTrackingCode/{code}")]
        public async Task<ActionResult<IEnumerable<Sale>>> FindWithTrackingCode(string code)
        {
            return await _context.Sale.Where(x => x.TrackingCode == code).ToListAsync();
        }

        [HttpPut("{saleId}")]
        public async Task<IActionResult> PutSale(int saleId)
        {
            var sale = await _context.Sale.FirstOrDefaultAsync(s => s.Id == saleId);

            if (saleId != sale.Id)
            {
                return BadRequest();
            }

            var SaleObj = await _context.Sale.FindAsync(saleId);
            SaleObj.Commission = sale.Commission;
            SaleObj.TotalPrice = sale.TotalPrice;
            SaleObj.CustomerId = sale.CustomerId;
            SaleObj.IsDeleted = sale.IsDeleted;
            SaleObj.OperatorId = sale.OperatorId;
            SaleObj.State = sale.State;
            SaleObj.TrackingCode = sale.TrackingCode;
            SaleObj.SaleDate = sale.SaleDate;
            _context.Entry(SaleObj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleExist(saleId))
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

        [AuthorizeOprator]
        [HttpPut("UpdateStateBySaleId/{saleId}")]
        public async Task<IActionResult> UpdateStateBySaleId(int saleId)
        {
            var sale = await _context.Sale.FirstOrDefaultAsync(s => s.Id == saleId);

            if (!SaleExist(saleId))
            {
                return NotFound("فاکتور یافت نشد");
            }

            if (saleId != sale.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var SaleObj = await _context.Sale.FindAsync(saleId);
            SaleObj.State = sale.State;

            _context.Entry(SaleObj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleExist(saleId))
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

        [AuthorizeOprator]
        [HttpPost("AddToCart")]
        public IActionResult AddToCart(int customerId, int sellerId, int productId, int productCount)
        {
            var seller = _context.Seller.SingleOrDefault(s => s.Id == sellerId);

            var customer = _context.Customer.SingleOrDefault(c => c.Id == customerId);

            var product = _context.Product.SingleOrDefault(p => p.Id == productId);

            long productPrice = product.PriceWithOff;

            if (!SellerExist(sellerId))
            {
                return NotFound("فروشنده یافت نشد");
            }

            if (!CustomerExist(customerId))
            {
                return NotFound("خریدار یافت نشد");
            }

            if (!ProductExist(productId))
            {
                return NotFound("کالا یافت نشد");
            }

            Sale sale = _context.Sale.SingleOrDefault(s =>
                s.CustomerId == customer.Id && !s.IsFinally && s.State == 1);

            if (sale == null)
            {
                Random random = new Random();
                int intTCode = random.Next(10000000, 99999999);
                string stringTCode = intTCode.ToString();

                sale = new Sale()
                {
                    CustomerId = customer.Id,
                    Commission = 0,
                    IsDeleted = false,
                    SellerId = sellerId,
                    OperatorId = 1,
                    State = 1,
                    TrackingCode = stringTCode,
                    SaleDate = DateTime.Now,
                    IsFinally = false,
                    TotalPrice = 0
                };

                _context.Sale.Add(sale);
                _context.SaveChanges();

                if (sellerId == 1)
                {
                    _context.SaleDetails.Add(new SaleDetail()
                    {
                        SaleId = sale.Id,
                        Count = productCount,
                        Price = productPrice,
                        SellerId = sellerId,
                        DateTime = DateTime.Now,
                        State = 1,
                        IsAdmin = true,
                        ProductId = productId,
                        Commission = product.Commission,
                        Takhfif = product.Takhfif,
                        ProductCode = product.Code,
                        ProductName = product.Name
                    });
                }
                else
                {
                    _context.SaleDetails.Add(new SaleDetail()
                    {
                        SaleId = sale.Id,
                        Count = productCount,
                        Price = productPrice,
                        SellerId = sellerId,
                        DateTime = DateTime.Now,
                        State = 1,
                        IsAdmin = false,
                        ProductId = productId,
                        Commission = product.Commission,
                        Takhfif = product.Takhfif,
                        ProductCode = product.Code,
                        ProductName = product.Name
                    });
                }

                ;

                UpdateTotalPriceSale(sale.Id);
                _context.SaveChanges();
            }
            else
            {
                var details = _context.SaleDetails.SingleOrDefault(d => d.SaleId == sale.Id && d.ProductId == productId);

                if (details == null)
                {
                    if (sellerId == 1)
                    {
                        _context.SaleDetails.Add(new SaleDetail()
                        {
                            SaleId = sale.Id,
                            Count = productCount,
                            Price = productPrice,
                            SellerId = sellerId,
                            DateTime = DateTime.Now,
                            State = 1,
                            IsAdmin = true,
                            ProductId = productId,
                            Commission = product.Commission,
                            Takhfif = product.Takhfif,
                            ProductCode = product.Code,
                            ProductName = product.Name
                        });
                    }
                    else
                    {
                        _context.SaleDetails.Add(new SaleDetail()
                        {
                            SaleId = sale.Id,
                            Count = productCount,
                            Price = productPrice,
                            SellerId = sellerId,
                            DateTime = DateTime.Now,
                            State = 1,
                            IsAdmin = false,
                            ProductId = productId,
                            Commission = product.Commission,
                            Takhfif = product.Takhfif,
                            ProductCode = product.Code,
                            ProductName = product.Name
                        });
                    }

                }

                else
                {
                    details.Count += productCount;
                    _context.Update(details);
                }

                UpdateTotalPriceSale(sale.Id);
            }

            UpdateTotalPriceSale(sale.Id);

            var result = JsonConvert.SerializeObject(sale, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return Ok(result);
        }

        [AuthorizeOprator]
        [HttpGet("UpdateTotalPriceSale")]
        public void UpdateTotalPriceSale(int saleId)
        {
            var sale = _context.Sale.Find(saleId);
            sale.TotalPrice = _context.SaleDetails.Where(s => s.SaleId == sale.Id).Select(d => d.Count * d.Price).Sum();
            sale.Commission = _context.SaleDetails.Where(s => s.SaleId == sale.Id && !s.IsAdmin).Select(d => d.Count * d.Commission).Sum();
            _context.Update(sale);
            _context.SaveChanges();
        }

        [AuthorizeOprator]
        [HttpPut("UpdateSellerCommision/{sellerId}")]
        public IActionResult UpdateCommisin(int sellerId)
        {
            var seller = _context.Seller.Where(s => s.Id == sellerId).FirstOrDefault();

            if (seller == null)
            {
                return NotFound("فروشنده یافت نشد");
            }

            var sales = _context.Sale.Where(s => s.SellerId == sellerId && s.IsFinally && s.State == 2);

            var saleDetails = _context.SaleDetails.Where(s => s.SellerId == sellerId && !s.IsAdmin && s.State == 2);

            if (sales.ToList().Count < 1)
            {
                return NotFound("فاکتوری برای این فروشنده به ثبت نرسیده است");
            }

            if (saleDetails.ToList().Count < 1)
            {
                return NotFound("جزئیات فاکتوری برای این فروشنده یافت نشد");
            }

            sales.ToList().ForEach(s=>s.State = 3);
            saleDetails.ToList().ForEach(s=>s.State = 3);

            //var commision = saleDetails.Select(s=>s.Commission);
            //var count = saleDetails.Select(s => s.Count);

            long updateCommision = sales.Select(s=>s.Commission).Sum();

            seller.Money += updateCommision;
            _context.Update(seller);
            _context.UpdateRange(sales);
            _context.UpdateRange(saleDetails);
            _context.SaveChanges();

            return NoContent();
        }

        [AuthorizeOprator]
        [HttpPut("SetFinallyTrue/{saleId}")]
        public async Task<IActionResult> UpdateSaleFinally(int saleId)
        {
            var sale = await _context.Sale.Include(s=>s.SaleDetails).Where(s => s.Id == saleId).FirstOrDefaultAsync();

            if (sale == null)
            {
                return NotFound("فاکتور یافت نشد");
            }

            var saleDetails = sale.SaleDetails.ToList();

            for (int i = 0; i < saleDetails.Count; i++)
            {
                var products = _context.Product.Find(saleDetails[i].ProductId);

                products.Amount -= saleDetails[i].Count;
                //saleDetails[i].Product.Amount -= saleDetails[i].Count;
            }

            sale.IsFinally = true;
            sale.State = 2;
            saleDetails.ToList().ForEach(s => s.State = 2);

            _context.Update(sale);
            _context.UpdateRange(saleDetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [AuthorizeOprator]
        [HttpDelete("DeleteFromCart")]
        public IActionResult DeleteFromCart(int saleId, int productId)
        {
            var sale = _context.Sale.Where(s => s.Id == saleId).FirstOrDefault();

            if (sale == null)
            {
                return NotFound("فاکتور مورد نظر یافت نشد");
            }

            var saleDetails = _context.SaleDetails.Where(s => s.SaleId == saleId).ToList();

            if (saleDetails.Contains(null))
            {
                return NotFound("جزئیات فاکتور یافت نشد");
            }

            var product = _context.SaleDetails.Where(s => s.ProductId == productId).FirstOrDefault();

            if (product == null)
            {
                return NotFound("کالای مورد نظر در جزئیات فاکتور موجود نیست");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.SaleDetails.Remove(product);
            sale.TotalPrice -= ((product.Count) * (product.Price));
            _context.Sale.Update(sale);

            if (sale.TotalPrice == 0)
            {
                _context.Remove(sale);
                _context.SaveChanges();
            }

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Sales/5
        [AuthorizeOprator]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Sale>> DeleteSale(int id)
        {
            var sale = await _context.Sale.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            _context.Sale.Remove(sale);
            await _context.SaveChangesAsync();

            return sale;
        }

        private bool SaleExist(int id)
        {
            return _context.Sale.Any(e => e.Id == id);
        }

        private bool CustomerExist(int customerId)
        {
            return _context.Customer.Any(c => c.Id == customerId);
        }

        private bool SellerExist(int id)
        {
            return _context.Seller.Any(s => s.Id == id);
        }

        private bool ProductExist(int id)
        {
            return _context.Product.Any(p => p.Id == id);
        }

        // GET: api/Sales
        /*[Authorize]
        [HttpGet("GetByChild")]
        public string GetSaleByChild(int pageNumber)//
        {
            int skip = pageNumber * 2;
            try
            {
                //var c = _context.Customer.ToList();
                //var o = _context.Oprator.ToList();
                //var s = _context.Seller.ToList();
                //var p = _context.Product.ToList();

                var sales = _context.Sale.Include(x => x.Operator).Include(x => x.Product).Include(x => x.Seller).Include(x => x.Customer).Skip(skip).Take(2).ToList();
                //foreach (var item in sales)
                //{
                //    item.Customer = new Customer();
                //        var customer = c.Where(x => x.Id == item.CustomerId).FirstOrDefault();
                //        if (customer != null)
                //            item.Customer = customer;



                //    item.Operator = new Oprator();
                //    var oprator = o.Where(x => x.Id == item.OperatorId).FirstOrDefault();
                //    if (oprator != null)
                //        item.Operator = oprator;

                //}



                //foreach (var item in sales)
                //{
                //    var customer = c.Where(x => x.Id == item.CustomerId).FirstOrDefault();
                //    if (customer != null)
                //        item.Customer = customer;


                //    var saller = _context.Seller.Find(item.SellerId);
                //    item.Seller = new Seller();
                //    item.Seller = saller;

                //    var product = _context.Product.Find(item.ProductId);
                //    item.Product = new Product();
                //    item.Product = product;

                //    var oprator = _context.Oprator.Find(item.OperatorId);
                //    item.Operator = new Oprator();
                //    item.Operator = oprator;
                //}
                //var j = JsonConvert.SerializeObject(sales);

                var e = JsonConvert.SerializeObject(sales, Formatting.None,
                         new JsonSerializerSettings()
                         {
                             ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                         });
                return e;
            }
            catch (Exception ex)
            {

                throw ex.InnerException;

            }

        }*/

        /*[HttpGet("BestSelling")]
        public async Task<ActionResult<IEnumerable<Product>>> GetBestSelling()
        {
            var sale = await _context.Sale.Where(x => x.State == 1)
                 .GroupBy(y => y.ProductId).Select(x => new
                 {
                     x.Key,
                     Count = x.Count()
                 }).OrderByDescending(x => x.Count).Take(5).ToListAsync();
            List<Product> products = new List<Product>();
            foreach (var item in sale)
            {
                var product = _context.Product.Find(item.Key);
                products.Add(product);

            }

            return products;

        }*/

        /*[HttpGet("Getbyorderid/{order}")]
        public async Task<ActionResult<IEnumerable<Sale>>> Getbyorderid(int order)
        {
            return await _context.Sale.Where(x => x.OrderId == order).ToListAsync();
        }

        [HttpGet("Findwithtrackingcode/{code}")]
        public async Task<ActionResult<IEnumerable<Sale>>> Findwithtrackingcode(string code)
        {
            return await _context.Sale.Where(x => x.TrackingCode == code).ToListAsync();
        }*/

        // PUT: api/Sales/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutSale(int id, Sale sale)
        {
            if (id != sale.Id)
            {
                return BadRequest();
            }
            var SaleObj = _context.Sale.Find(id);
            SaleObj.Commission = sale.Commission;
            SaleObj.Totalprice = sale.Totalprice;
            SaleObj.CustomerId = sale.CustomerId;
            SaleObj.IsDeleted = sale.IsDeleted;
            SaleObj.OperatorId = sale.OperatorId;
            SaleObj.ProductId = sale.ProductId;
            SaleObj.SellerId = sale.SellerId;
            SaleObj.State = sale.State;
            SaleObj.TrackingCode = sale.TrackingCode;
            SaleObj.SaleDate = sale.SaleDate;
            _context.Entry(SaleObj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        /*[HttpPut("PutSalewithorder/{id}")]
        public async Task<IActionResult> PutSalewithorder(int id, Sale sale)
        {
            if (id != sale.Id)
            {
                return BadRequest();
            }
            var SaleObj = _context.Sale.Find(id);
            SaleObj.Commission = sale.Commission;
            SaleObj.CustomerId = sale.CustomerId;
            SaleObj.IsDeleted = sale.IsDeleted;
            SaleObj.OperatorId = sale.OperatorId;
            SaleObj.ProductId = sale.ProductId;
            SaleObj.SellerId = sale.SellerId;
            SaleObj.State = sale.State;
            SaleObj.Totalprice = sale.Totalprice;
            SaleObj.TrackingCode = sale.TrackingCode;
            SaleObj.SaleDate = sale.SaleDate;
            _context.Entry(SaleObj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/Sales
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /*[HttpPost]
        public string PostSale(SaleViewModel sale)
        {
            int o = 0;
            List<Sale> orderNum = new List<Sale>();
            orderNum= _context.Sale.ToList();
            if (orderNum == null)
                 o = 1;
            else
                o = _context.Sale.OrderByDescending(x => x.OrderId).FirstOrDefault().OrderId.Value+1;
            //int o = orderNum.Value + 1;

            foreach (var item in sale.ProductIds)
            {
                var entity = _context.Sale.Add(new Sale
                {
                    Commission = sale.Commission,
                    CustomerId = sale.CustomerId,
                    IsDeleted = sale.IsDeleted,
                    OperatorId = sale.OperatorId,
                    ProductId = item,
                    SellerId = sale.SellerId,
                    State = sale.State,
                    Totalprice = sale.Totalprice,          
                    TrackingCode = sale.TrackingCode,
                    SaleDate = sale.SaleDate,
                    OrderId = o

                });
            }

            _context.SaveChangesAsync();
            //sale.Id = entity.Entity.Id;
            //return CreatedAtAction("GetSale", new { id = sale.Id }, sale);
            return "orderId: "+o;
        }*/
    }
}
