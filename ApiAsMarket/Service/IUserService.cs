using ApiAsMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAsMarket.Service
{
    public interface IUserService
    {
        AdminInfo Authenticate(AuthenticateRequest model);
        Oprator AuthenticateOperator(AuthenticateRequest model);
        Seller AuthenticateSeller(AuthenticateRequest model);
        Customer AuthenticateCustomer(AuthenticateRequest model);

        AdminInfo GetById(int id);

        Oprator GetByIdOprator(int id);
        Seller GetByIdSeller(int id);
        Customer GetByIdCustomer(int id);
    }
}
