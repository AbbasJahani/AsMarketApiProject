using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAsMarket.Models
{
    public class SaleDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int SaleId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public long Price { get; set; }

        [Required]
        public int SellerId { get; set; }

        public long Commission { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public int? Takhfif { get; set; }

        public DateTime? DateTime { get; set; }

        /// <summary>
        /// 1 -> Processing
        /// 2 -> IsFinally=True
        /// 3 -> CommisionPaid
        /// </summary>
        public int State { get; set; }

        public bool IsAdmin { get; set; }

        public virtual Sale Sale { get; set; }
        public virtual Product Product { get; set; }
        public virtual Seller Seller { get; set; }
    }
}
