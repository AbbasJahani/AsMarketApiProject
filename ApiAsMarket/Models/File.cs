using System;
using System.Collections.Generic;

namespace ApiAsMarket.Models
{
    public partial class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public string Link { get; set; }
        public bool? IsDeleted { get; set; }
       public string Image { get; set; }
        public string Description { get; set; }
    }
}
