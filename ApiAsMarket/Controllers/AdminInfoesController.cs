using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiAsMarket.Models;
using ApiAsMarket.Service;
using ApiAsMarket.Core;
using ApiAsMarket.ViewModels;

namespace ApiAsMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminInfoesController : ControllerBase
    {
        private readonly ApiAsMarketContext _context;
        private IUserService _userService;

        public AdminInfoesController(ApiAsMarketContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpPost("Authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        // GET: api/AdminInfoes
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminInfo>>> GetAdminInfo()
        {
            return await _context.AdminInfo.ToListAsync();
        }


        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword(string mobileOrUsername)
        {
            var response = _context.AdminInfo.Where(x => x.Mobile == mobileOrUsername || x.UserName == mobileOrUsername).SingleOrDefault();
            if (response == null)
                return BadRequest(new { message = "کاربر ثبت نام نکرده است" });
            SendSmsService sendSmsService = new SendSmsService();
            sendSmsService.SendForgotPassWordSms("09137056390", "2", "5", "8");
            return Ok(response);
        }

        [HttpPost("RegisterCode")]
        public IActionResult RegisterCode(string mobile)
        {
            var response = _context.AdminInfo.Where(x => x.Mobile == mobile && x.IsActive == false).SingleOrDefault();
            if (response == null)
                return BadRequest(new { message = "این شماره موبایل وجود ندارد ویا قبلا تایید شده است" });
            return Ok("لینک فعال سازی برای شما پیامک شد");
        }
        // GET: api/AdminInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminInfo>> GetAdminInfo(int id)
        {
            var adminInfo = await _context.AdminInfo.FindAsync(id);

            if (adminInfo == null)
            {
                return NotFound();
            }

            return adminInfo;
        }

        // PUT: api/AdminInfoes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdminInfo(int id, AdminInfoViewModel adminInfo)
        {
            if (id != adminInfo.Id)
            {
                return BadRequest();
            }
            var AdminInfoObj = _context.AdminInfo.Find(id);
            AdminInfoObj.FullName = adminInfo.FullName;
            AdminInfoObj.ImageUrl = adminInfo.ImageUrl;
            AdminInfoObj.IsActive = adminInfo.IsActive;
            AdminInfoObj.IsDeleted = adminInfo.IsDeleted;
            AdminInfoObj.Mobile = adminInfo.Mobile;
            AdminInfoObj.Password = adminInfo.Password;
            AdminInfoObj.UserName = adminInfo.UserName;
            _context.Entry(AdminInfoObj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminInfoExists(id))
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

        // POST: api/AdminInfoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AdminInfo>> PostAdminInfo(AdminInfoViewModel adminInfo)
        {

            var entity = _context.AdminInfo.Add(new AdminInfo
            {
                FullName = adminInfo.FullName,
                ImageUrl = adminInfo.ImageUrl,
                IsActive = adminInfo.IsActive,
                IsDeleted = adminInfo.IsDeleted,
                Mobile = adminInfo.Mobile,
                Password = adminInfo.Password,
                UserName = adminInfo.UserName
            });
            await _context.SaveChangesAsync();
            adminInfo.Id = entity.Entity.Id;
            return CreatedAtAction("GetAdminInfo", new { id = adminInfo.Id }, adminInfo);
        }

        // DELETE: api/AdminInfoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AdminInfo>> DeleteAdminInfo(int id)
        {
            var adminInfo = await _context.AdminInfo.FindAsync(id);
            if (adminInfo == null)
            {
                return NotFound();
            }

            _context.AdminInfo.Remove(adminInfo);
            await _context.SaveChangesAsync();

            return adminInfo;
        }

        private bool AdminInfoExists(int id)
        {
            return _context.AdminInfo.Any(e => e.Id == id);
        }
       
    }
}
