using System;
using System.Collections.Generic;

namespace ApiAsMarket.ViewModels
{
    public  class ProductCategoryViewModel
    {
     

        public int Id { get; set; }
        public string Name { get; set; }
        public string Image1 { get; set; }

        public bool? IsDeleted { get; set; }

      
    }
}
