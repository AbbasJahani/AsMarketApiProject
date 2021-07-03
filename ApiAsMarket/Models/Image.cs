using System;
using System.Collections.Generic;

namespace ApiAsMarket.Models
{
    public partial class Payment
    {
        public int Id { get; set; }
        public DateTime  Date { get; set; }
        public long Amount { get; set; }
        public string Authority { get; set; }
        public string Status { get; set; }

    }
}
