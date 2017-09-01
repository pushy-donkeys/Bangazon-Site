using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWebsite.Models.ViewModels
{
    public class ProductListViewModel
    {
        [Display(Name = "Make a Selection")]
        public IEnumerable<Product> product {get;set;}
    }
}
