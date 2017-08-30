using BangazonWebsite.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWebsite.Models.ViewModels
{
    public class ProductTypeViewModel
    {
        public IEnumerable<ProductType> ProductTypes { get; set; }
    }
}