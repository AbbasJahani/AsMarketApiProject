using System;
using System.Collections.Generic;

namespace ApiAsMarket.ViewModels
{
    public  class AdminInfoViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string ImageUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? IsActive { get; set; }
      
        public bool? IsDeleted { get; set; }
    }
}
