using System;
using System.Collections.Generic;

namespace ApiAsMarket.ViewModels
{
    public  class PosterViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ProductId { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public bool? IsDeleted { get; set; }
        public string Caption { get; set; }

    }
}
