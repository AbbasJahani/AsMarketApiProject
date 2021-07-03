using ApiAsMarket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using ZarinPal;


namespace ApiAsMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly ApiAsMarketContext _context;

        public PaymentController(ApiAsMarketContext context)
        {
            _context = context;

        }

        [HttpGet]
        public async Task<ActionResult> Get(long Amount, string Description)
        {

            //Install-Package ZarinPal-SDK -Version 0.0.8
            ZarinPal.ZarinPal zarinpal = ZarinPal.ZarinPal.Get();
            String MerchantID = "622969e7-8b3e-4f34-8a7a-763fdc01cd82";
            String CallbackURL = "http://localhost:14095/api/payment/verification";
            ZarinPal.PaymentRequest pr = new ZarinPal.PaymentRequest(MerchantID, Amount, CallbackURL, Description);
            var res = zarinpal.InvokePaymentRequest(pr);

            var entity = _context.Payment.Add(new Payment
            {
                Date = DateTime.Now,
                Amount = Amount,
                Authority = res.Authority,

            });
            _context.SaveChanges();

            return Redirect(res.PaymentURL);
        }
        [HttpGet("Verification")]
        public ActionResult Verification(string Authority, string Status)
        {
            var entity = _context.Payment.Where(x => x.Authority == Authority).FirstOrDefault();
            if (entity != null)
            {
                ZarinPal.ZarinPal zarinpal = ZarinPal.ZarinPal.Get();
                String MerchantID = "622969e7-8b3e-4f34-8a7a-763fdc01cd82";
                var verificationRequest = new ZarinPal.PaymentVerification(MerchantID, entity.Amount, Authority);
                var verificationResponse = zarinpal.InvokePaymentVerification(verificationRequest);

                entity.Status = Status;

                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChangesAsync();
            }
            MyTemp temp = new MyTemp { Authority = Authority, Id = entity.Id, Status = Status };
            string output = JsonConvert.SerializeObject(temp);
            return Ok(output);
        }
        /// <summary>
        /// اعتبار سنجی خرید
        /// </summary>
        [HttpGet("Validate")]
        public string Validate(string authority, long amount)
        {
            ZarinPal.PaymentVerification pv = new ZarinPal.PaymentVerification("622969e7-8b3e-4f34-8a7a-763fdc01cd82", amount, authority);
            ZarinPal.ZarinPal zarinpal = ZarinPal.ZarinPal.Get();
            var res = zarinpal.InvokePaymentVerification(pv);
            string output = JsonConvert.SerializeObject(res);
            return output;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = await _context.Payment.FindAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return payment;
        }

    }
}

public class MyTemp
{
    public int Id { get; set; }
    public string Authority { get; set; }
    public string Status { get; set; }
}
