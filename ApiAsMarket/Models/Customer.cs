using System;
using System.Collections.Generic;

namespace ApiAsMarket.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Sale = new HashSet<Sale>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string NationalCode { get; set; }
        public string Phone { get; set; }
        public long? Commission { get; set; }
        public bool? IsDeleted { get; set; }
        public string Family { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Sale> Sale { get; set; }
    }
}
