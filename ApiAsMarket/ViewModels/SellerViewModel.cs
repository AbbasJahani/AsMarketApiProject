using System;
using System.Collections.Generic;

namespace ApiAsMarket.ViewModels
{
    public  class SellerViewModel
    {
     
        public int Id { get; set; }
        public string Name { get; set; }
        public int? PersonalCode { get; set; }
        public bool? IsDeleted { get; set; }
        public string Mobile { get; set; }
        public string NationalCode { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public long Commission { get; set; }
        public string Image { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public long Money { get; set; }
        public string Family { get; set; }

    }
}
