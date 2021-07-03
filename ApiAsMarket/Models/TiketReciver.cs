using System;
using System.Collections.Generic;

namespace ApiAsMarket.Models
{
    public partial class TiketReciver
    {
        public int Id { get; set; }
        public int? ReciverId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
