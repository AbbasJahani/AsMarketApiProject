using System;
using System.Collections.Generic;

namespace ApiAsMarket.ViewModels
{
    public  class TicketViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int? UserId { get; set; }
        public DateTime? SendDate { get; set; }
        public string Subject { get; set; }
        public int? UserType { get; set; }
        public bool? IsDeleted { get; set; }
        public string Mobile { get; set; }
        public int? Status { get; set; }
    }
}
