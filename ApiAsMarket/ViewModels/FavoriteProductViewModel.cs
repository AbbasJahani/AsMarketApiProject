using System;
using System.Collections.Generic;

namespace ApiAsMarket.ViewModels
{
    public  class FavoriteProductViewModel
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? UserId { get; set; }
        public bool? IsDeleted { get; set; }

      
    }
}
