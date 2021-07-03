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
using System.Globalization;
using Newtonsoft.Json;

namespace ApiAsMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApiAsMarketContext _context;
        private IUserService _userService;

        public CustomersController(ApiAsMarketContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        [HttpPost("Authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.AuthenticateCustomer(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }


        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword(string mobileOrUsername)
        {
            var response = _context.Customer.Where(x => x.Mobile == mobileOrUsername || x.UserName == mobileOrUsername).SingleOrDefault();
            if (response == null)
                return BadRequest(new { message = "کاربر ثبت نام نکرده است" });
            return Ok(response);
        }

        [HttpPost("RegisterCode")]
        public IActionResult RegisterCode(string mobile)
        {
            var response = _context.Customer.Where(x => x.Mobile == mobile && x.IsActive == false).SingleOrDefault();
            if (response == null)
                return BadRequest(new { message = "این شماره موبایل وجود ندارد ویا قبلا تایید شده است" });
            return Ok("لینک فعال سازی برای شما پیامک شد");
        }

        //[AuthorizeCustomer]
        //// GET: api/Customers
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
        //{
        //    return await _context.Customer.ToListAsync();
        //}

        //[AuthorizeCustomer]
        // GET: api/Customers
        [HttpGet]
        public string GetCustomer()
        {
            var customer=  _context.Customer.Include(x=>x.Sale).ToList();
            var e = JsonConvert.SerializeObject(customer, Formatting.None,
           new JsonSerializerSettings()
           {
               ReferenceLoopHandling = ReferenceLoopHandling.Ignore
           });
            return e;
        }

        //[AuthorizeCustomer]
        //// GET: api/Customers/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Customer>> GetCustomer(int id)
        //{
        //    var customer = await _context.Customer.FindAsync(id);

        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return customer;
        //}

         
        // GET: api/Customers/5
        [HttpGet("{id}")]
        public string GetCustomer(int id)
        {
            var customer =  _context.Customer.Where(x=>x.Id==id).Include(x=>x.Sale);
            var e = JsonConvert.SerializeObject(customer, Formatting.None,
           new JsonSerializerSettings()
           {
               ReferenceLoopHandling = ReferenceLoopHandling.Ignore
           });
            return e;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerViewModel customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }
            var CustomerObj = _context.Customer.Find(id);
            CustomerObj.BirthDate = customer.BirthDate;
            CustomerObj.Mobile = customer.Mobile;
            CustomerObj.IsDeleted = customer.IsDeleted;
            CustomerObj.Address = customer.Address;
            CustomerObj.Commission = customer.Commission;
            CustomerObj.Email = customer.Email;
            CustomerObj.Family = customer.Family;
            CustomerObj.Image = customer.Image;
            CustomerObj.Name = customer.Name;
            CustomerObj.NationalCode = customer.NationalCode;
            CustomerObj.Phone = customer.Phone;
            CustomerObj.PostalCode = customer.PostalCode;

            CustomerObj.UserName = customer.UserName;
            CustomerObj.Password = customer.Password;
            _context.Entry(CustomerObj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(CustomerViewModel customer)
        {
            var entity = _context.Customer.Add(new Customer
            {
                BirthDate = customer.BirthDate,
                Mobile = customer.Mobile,
                IsDeleted = customer.IsDeleted,
                Address = customer.Address,
                Commission = customer.Commission,
                Email = customer.Email,
                Family = customer.Family,
                Image = customer.Image,
                Name = customer.Name,
                NationalCode = customer.NationalCode,
                Phone = customer.Phone,
                PostalCode = customer.PostalCode,
                UserName=customer.UserName,
                Password=customer.Password

            });
            await _context.SaveChangesAsync();
            customer.Id = entity.Entity.Id;
            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }
    }
}
