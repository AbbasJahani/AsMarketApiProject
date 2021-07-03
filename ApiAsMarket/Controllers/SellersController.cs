using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiAsMarket.Models;
using ApiAsMarket.ViewModels;
using ApiAsMarket.Service;
using ApiAsMarket.Core;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace ApiAsMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellersController : ControllerBase
    {
        private readonly ApiAsMarketContext _context;
        private IUserService _userService;
        public SellersController(ApiAsMarketContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpPost("Authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.AuthenticateSeller(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }


        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword(string mobileOrUsername)
        {
            var response = _context.Seller.Where(x => x.Mobile == mobileOrUsername || x.UserName == mobileOrUsername).SingleOrDefault();
            if (response == null)
                return BadRequest(new { message = "کاربر ثبت نام نکرده است" });
            return Ok(response);
        }

        [HttpPost("RegisterCode")]
        public IActionResult RegisterCode(string mobile)
        {
            var response = _context.Seller.Where(x => x.Mobile == mobile && x.IsActive == false).SingleOrDefault();
            if (response == null)
                return BadRequest(new { message = "این شماره موبایل وجود ندارد ویا قبلا تایید شده است" });
            return Ok("لینک فعال سازی برای شما پیامک شد");
        }

        //[AuthorizeSeller]
        // GET: api/Sellers
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Seller>>> GetSeller()
        //{

        //        return await _context.Seller.ToListAsync();



        //}
        [HttpGet]
        public  string GetSeller()
        {
            var seller=  _context.Seller.Include(x=>x.Sale).ToList();
            var e = JsonConvert.SerializeObject(seller, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
            return e;

        }

        //[AuthorizeSeller]
        //// GET: api/Sellers/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Seller>> GetSeller(int id)
        //{
        //    var seller = await _context.Seller.FindAsync(id);

        //    if (seller == null)
        //    {
        //        return NotFound();
        //    }

        //    return seller;
        //}

        
        // GET: api/Sellers/5
        [HttpGet("{id}")]
        public string GetSeller(int id)
        {
            

            var seller = _context.Seller.Where(x=>x.Id==id).Include(x => x.Sale);
            
            var e = JsonConvert.SerializeObject(seller, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
            return e;
        }


        [HttpGet("GetSellersByCountOfSalesInMonth")]
        public async Task<ActionResult<IEnumerable<SallerCountOfSales>>> GetSellersByCountOfSalesInMonth()
        {
            
            var seller = await _context.Seller.ToListAsync();
            List<SallerCountOfSales> sallerCountOfSales = new List<SallerCountOfSales>();
            foreach (var item in seller)
            {
                var sales = _context.Sale.Where(x => x.SellerId == item.Id).ToListAsync();
                if(sales != null)
                {
                    SallerCountOfSales sallerCountOfSalesItem = new SallerCountOfSales();
                    sallerCountOfSalesItem.Id = item.Id;
                    sallerCountOfSalesItem.Name = item.Name;
                    sallerCountOfSalesItem.PersonalCode = item.PersonalCode;
                    sallerCountOfSalesItem.IsDeleted = item.IsDeleted;
                    sallerCountOfSalesItem.Mobile = item.Mobile;
                    sallerCountOfSalesItem.NationalCode = item.NationalCode;
                    sallerCountOfSalesItem.Email = item.Email;
                    sallerCountOfSalesItem.Address = item.Address;
                    sallerCountOfSalesItem.Phone = item.Phone;
                    sallerCountOfSalesItem.Commission = item.Commission;
                    sallerCountOfSalesItem.Image = item.Image;
                    sallerCountOfSalesItem.CountSale = sales.Result.Count();
                    sallerCountOfSalesItem.Money = item.Money;
                    sallerCountOfSalesItem.Family = item.Family;
                    if (sales.Result.Where(x => x.SaleDate.Value.Month == DateTime.Now.Month).Any()) {
                        //var tt = sales.Result.Where(x => x.SaleDate.Value.Month == DateTime.Now.Month).ToList();
                        sallerCountOfSales.Add(sallerCountOfSalesItem);
                    }
                }
            }

            if (seller == null)
            {
                return NotFound();
            }

            return sallerCountOfSales.OrderByDescending(x=>x.CountSale).ToList();
        }

        [AuthorizeSeller]
        [HttpGet("GetAllSellerSales/{sellerId}")]
        public async Task<ActionResult<string>> GetASellerSales(int sellerId)
        {
            var seller = await _context.Seller.Where(s => s.Id == sellerId).FirstOrDefaultAsync();

            if (seller == null)
            {
                return NotFound("فروشنده یافت نشد");
            }

            var sales = await _context.Sale.Where(s => s.SellerId == sellerId).OrderByDescending(s=>s.Id).ToListAsync();

            if (sales.Count < 1)
            {
                return NotFound("این فروشنده فاکتوری ندارد");
            }

            var result = JsonConvert.SerializeObject(sales, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return result;
        }

        // PUT: api/Sellers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeller(int id, SellerViewModel seller)
        {
            if (id != seller.Id)
            {
                return BadRequest();
            }
            var SellerObj = _context.Seller.Find(id);
            SellerObj.Address = seller.Address;
            SellerObj.Commission = seller.Commission;
            SellerObj.Email = seller.Email;
            SellerObj.Image = seller.Image;
            SellerObj.IsDeleted = seller.IsDeleted;
            SellerObj.Mobile = seller.Mobile;
            SellerObj.NationalCode = seller.NationalCode;
            SellerObj.PersonalCode = seller.PersonalCode;
            SellerObj.Name = seller.Name;
            SellerObj.Phone = seller.Phone;
            SellerObj.Money = seller.Money;
            SellerObj.Family = seller.Family;
            SellerObj.UserName = seller.UserName;
            SellerObj.Password = seller.Password;
            _context.Entry(SellerObj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SellerExists(id))
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
        [HttpPut("SellerCheckout/{sellerId}")]
        public IActionResult UpdateSellerMoney(int sellerId)
        {
            var seller = _context.Seller.Where(s => s.Id == sellerId).FirstOrDefault();

            if (seller == null)
            {
                return NotFound("فروشنده یافت نشد");
            }

            if (seller.Money == 0)
            {
                return BadRequest("موجودی فروشنده صفر است");
            }

            seller.Money = 0;
            _context.Update(seller);
            _context.SaveChanges();

            return Ok("پورسانت فروشنده پرداخت و موجودی وی صفر شد");
        }

        // POST: api/Sellers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Seller>> PostSeller(SellerViewModel seller)
        {
            var entity = _context.Seller.Add(new Seller
            {
                Address = seller.Address,
                Commission = seller.Commission,
                Email = seller.Email,
                Image = seller.Image,
                IsDeleted = seller.IsDeleted,
                Mobile = seller.Mobile,
                NationalCode = seller.NationalCode,
                PersonalCode = seller.PersonalCode,
                Name = seller.Name,
                Phone = seller.Phone,
                Money=seller.Money,
                Family=seller.Family,
                UserName=seller.UserName,
                Password=seller.Password

            });
            await _context.SaveChangesAsync();
            seller.Id = entity.Entity.Id;
            return CreatedAtAction("GetSeller", new { id = seller.Id }, seller);
        }

        // DELETE: api/Sellers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Seller>> DeleteSeller(int id)
        {
            var seller = await _context.Seller.FindAsync(id);
            if (seller == null)
            {
                return NotFound();
            }

            _context.Seller.Remove(seller);
            await _context.SaveChangesAsync();

            return seller;
        }

        private bool SellerExists(int id)
        {
            return _context.Seller.Any(e => e.Id == id);
        }
    }
}
