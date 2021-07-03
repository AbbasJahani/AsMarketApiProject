using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiAsMarket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAsMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ApiAsMarketContext _context;
        public ImageController(ApiAsMarketContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Image>>> GetAll()
        {
            return await _context.Image.ToListAsync();
        }


        [HttpPost("UploadImage")]
        public  string UploadImage([FromBody] GetImage imageBytes)
        {
           // var imageBytes = System.Convert.FromBase64String(imageBase64);
            //byte[] imageBytes = System.IO.File.ReadAllBytes(@"F:\_CSharpProject\ApiAsMarket\ApiAsMarket\a\example.jpg");
            string fileName = Guid.NewGuid().ToString() + ".jpg";
            System.IO.File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + @"\images\" + fileName, imageBytes.imageBytes);

            var entity = _context.Image.Add(new Image
            {
                Name = fileName
            });
             _context.SaveChanges();
            return fileName;
        }
    }


    public class GetImage
    {
       public byte[] imageBytes { get; set; }
    }
}
