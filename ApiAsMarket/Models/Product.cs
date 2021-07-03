using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiAsMarket.Models
{
    public partial class Product
    {
        public Product()
        {
            FavoriteProduct = new HashSet<FavoriteProduct>();
            Poster = new HashSet<Poster>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public string Code { get; set; }
        public string Attribute { get; set; }
        public int? Amount { get; set; }
        public int? Takhfif { get; set; }
        public long Price { get; set; }
        public long PriceWithOff { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public long Commission { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ProductCategory Category { get; set; }
        public virtual ICollection<FavoriteProduct> FavoriteProduct { get; set; }
        public virtual ICollection<Poster> Poster { get; set; }
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
    }
}
