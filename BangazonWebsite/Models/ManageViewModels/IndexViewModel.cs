using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BangazonWebsite.Models.ManageViewModels
{
    public class IndexViewModel
    {

        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<PaymentType> PaymentTypes { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<Product> Products { get; set; }
        public IndexViewModel()
        {
            ApplicationUser = new ApplicationUser();
        }
    }
}
