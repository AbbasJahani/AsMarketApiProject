using ApiAsMarket.Core;
using ApiAsMarket.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiAsMarket.Service
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly ApiAsMarketContext _context;

        public UserService(IOptions<AppSettings> appSettings, ApiAsMarketContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }

        public AdminInfo Authenticate(AuthenticateRequest model)
        {
            var user = _context.AdminInfo.SingleOrDefault(x => x.UserName == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AdminInfo { Id=user.Id,FullName=user.FullName,Mobile=user.Mobile,IsDeleted=user.IsDeleted,IsActive=user.IsActive,ImageUrl=user.ImageUrl,Token=token};
        }
        public Oprator AuthenticateOperator(AuthenticateRequest model)
        {
            var user = _context.Oprator.SingleOrDefault(x => x.UserName == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtTokenOprator(user);

            return new Oprator { Id = user.Id, Mobile = user.Mobile,Family=user.Family,Name=user.Name,Image=user.Image,NationalCode=user.NationalCode,PersonalCode=user.PersonalCode, IsDeleted = user.IsDeleted, IsActive = user.IsActive, Token = token };
        }
        public Seller AuthenticateSeller(AuthenticateRequest model)
        {
            var user = _context.Seller.SingleOrDefault(x => x.UserName == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtTokenSeller(user);

            return new Seller { Id = user.Id,  Mobile = user.Mobile,Name=user.Name,Address=user.Address,PersonalCode=user.PersonalCode,Phone=user.Phone,Image=user.Image,NationalCode=user.NationalCode,Email=user.Email,Commission=user.Commission, IsDeleted = user.IsDeleted, Token = token };
        }
        public Customer AuthenticateCustomer(AuthenticateRequest model)
        {
            var user = _context.Customer.SingleOrDefault(x => x.UserName == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtTokenCustomer(user);

            return new Customer { Id = user.Id, Mobile = user.Mobile,Name=user.Name,Family=user.Family,Email=user.Email,BirthDate=user.BirthDate,Phone=user.Phone,Commission=user.Commission,Image=user.Image,Address=user.Address,NationalCode=user.NationalCode,PostalCode=user.PostalCode, IsDeleted = user.IsDeleted,Token = token };
        }



        public AdminInfo GetById(int id)
        {
            return _context.AdminInfo.Find(id);
        }

        public Oprator GetByIdOprator(int id)
        {
            return _context.Oprator.Find(id);
        }

        public Seller GetByIdSeller(int id)
        {
            return _context.Seller.Find(id);
        }

        public Customer GetByIdCustomer(int id)
        {
            return _context.Customer.Find(id);
        }

        // helper methods

        private string generateJwtToken(AdminInfo user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string generateJwtTokenCustomer(Customer user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string generateJwtTokenOprator(Oprator user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string generateJwtTokenSeller(Seller user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
