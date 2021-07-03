using System;
using System.Collections.Generic;

namespace ApiAsMarket.ViewModels
{
    public  class SaleViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        /// <summary>
        /// 1 --> Processing
        /// 2 --> IsFinally=True
        /// 3 --> CommisionPaidToSeller
        /// </summary>
        public int State { get; set; }
        public DateTime? SaleDate { get; set; }
        public long Commission { get; set; }
        public long TotalPrice { get; set; }
        public bool? IsDeleted { get; set; }
        public int SellerId { get; set; }
        public int? OperatorId { get; set; }
        public string TrackingCode { get; set; }
        public bool IsFinally { get; set; }
    }
}
