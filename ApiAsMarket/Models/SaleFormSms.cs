using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAsMarket.Models
{
    public partial class SaleFormSms
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string CustomerMobile { get; set; }
        public int OperatorId { get; set; }
        public int SellerId { get; set; }
        public int State { get; set; }

    }
}
