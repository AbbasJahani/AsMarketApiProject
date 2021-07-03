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
using Newtonsoft.Json;

namespace ApiAsMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpratorsController : ControllerBase
    {
        private readonly ApiAsMarketContext _context;
        private IUserService _userService;

        public OpratorsController(ApiAsMarketContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpPost("Authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.AuthenticateOperator(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword(string mobileOrUsername)
        {
            var response = _context.Oprator.Where(x => x.Mobile == mobileOrUsername || x.UserName == mobileOrUsername).SingleOrDefault();
            if (response == null)
                return BadRequest(new { message = "کاربر ثبت نام نکرده است" });
            return Ok(response);
        }

        [HttpPost("RegisterCode")]
        public IActionResult RegisterCode(string mobile)
        {
            var response = _context.Oprator.Where(x => x.Mobile == mobile && x.IsActive == false).SingleOrDefault();
            if (response == null)
                return BadRequest(new { message = "این شماره موبایل وجود ندارد ویا قبلا تایید شده است" });
            return Ok("لینک فعال سازی برای شما پیامک شد");
        }

        //[AuthorizeOprator]
        //// GET: api/Oprators
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Oprator>>> GetOprator()
        //{
        //    try
        //    {
        //        return await _context.Oprator.ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex.InnerException;
        //    }
        //}

        [AuthorizeOprator]
        // GET: api/Oprators
        [HttpGet]
        public string GetOprator()
        {
            
                var oprator=  _context.Oprator.Include(x=>x.Sale).ToList();
            var e = JsonConvert.SerializeObject(oprator, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            return e;
        }

        //[AuthorizeOprator]
        //// GET: api/Oprators/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Oprator>> GetOprator(int id)
        //{
        //    var oprator = await _context.Oprator.FindAsync(id);

        //    if (oprator == null)
        //    {
        //        return NotFound();
        //    }

        //    return oprator;
        //}
        [AuthorizeOprator]
        // GET: api/Oprators/5
        [HttpGet("{id}")]
        public string GetOprator(int id)
        {
            var oprator =  _context.Oprator.Where(x=>x.Id==id).Include(x=>x.Sale);

            var e = JsonConvert.SerializeObject(oprator, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            return e;
        }

        // PUT: api/Oprators/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOprator(int id, OpratorViewModel oprator)
        {
            if (id != oprator.Id)
            {
                return BadRequest();
            }
            var OpratorObj = _context.Oprator.Find(id);
            OpratorObj.Name = oprator.Name;
            OpratorObj.IsDeleted = oprator.IsDeleted;
            OpratorObj.Family = oprator.Family;
            OpratorObj.Image = oprator.Image;
            OpratorObj.IsActive = oprator.IsActive;
            OpratorObj.Mobile = oprator.Mobile;
            OpratorObj.NationalCode = oprator.NationalCode;
            OpratorObj.Password = oprator.Password;
            OpratorObj.PersonalCode = oprator.PersonalCode;
            OpratorObj.UserName = oprator.UserName;
            _context.Entry(OpratorObj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OpratorExists(id))
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

        // POST: api/Oprators
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Oprator>> PostOprator(OpratorViewModel oprator)
        {
            var entity = _context.Oprator.Add(new Oprator
            {
                Name = oprator.Name,
                IsDeleted = oprator.IsDeleted,
                Family = oprator.Family,
                Image = oprator.Image,
                IsActive = oprator.IsActive,
                Mobile = oprator.Mobile,
                NationalCode = oprator.NationalCode,
                Password = oprator.Password,
                PersonalCode = oprator.PersonalCode,
                UserName = oprator.UserName
            });
            await _context.SaveChangesAsync();
            oprator.Id = entity.Entity.Id;
            return CreatedAtAction("GetOprator", new { id = oprator.Id }, oprator);
        }

        // DELETE: api/Oprators/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Oprator>> DeleteOprator(int id)
        {
            var oprator = await _context.Oprator.FindAsync(id);
            if (oprator == null)
            {
                return NotFound();
            }

            _context.Oprator.Remove(oprator);
            await _context.SaveChangesAsync();

            return oprator;
        }

        private bool OpratorExists(int id)
        {
            return _context.Oprator.Any(e => e.Id == id);
        }
    }
}
