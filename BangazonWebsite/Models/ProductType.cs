using BangazonWebsite.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWebsite.Models
{
    public class ProductType
    {
        [Key]
        public int ProductTypeId { get; set; }

        [StringLength(255)]
        [Display(Name="Category")]
        public string Label { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
