using System;
using System.Collections.Generic;

namespace ApiAsMarket.Models
{
    public partial class FavoriteProduct
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? UserId { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Product Product { get; set; }
    }
}
