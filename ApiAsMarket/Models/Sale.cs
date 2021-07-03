using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAsMarket.Models
{
    public partial class Sale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        public virtual Customer Customer { get; set; }
        public virtual Oprator Operator { get; set; }
        public virtual Seller Seller { get; set; }
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
    }
}
