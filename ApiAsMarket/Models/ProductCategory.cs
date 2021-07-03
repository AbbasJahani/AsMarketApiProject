using System;
using System.Collections.Generic;

namespace ApiAsMarket.Models
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            Product = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Image1 { get; set; }

        public bool? IsDeleted { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
