using System;
using System.Collections.Generic;

namespace ApiAsMarket.Models
{
    public partial class Oprator
    {
        public Oprator()
        {
            Sale = new HashSet<Sale>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? PersonalCode { get; set; }
        public string Mobile { get; set; }
        public string NationalCode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? IsDeleted { get; set; }
        public string Family { get; set; }
        public string Image { get; set; }
        public bool? IsActive { get; set; }
        public string Token { get; set; }

        public virtual ICollection<Sale> Sale { get; set; }
    }
}
