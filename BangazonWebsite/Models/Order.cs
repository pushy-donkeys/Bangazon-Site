using BangazonWebsite.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWebsite.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int PaymentTypeId { get; set; }

        public PaymentType PaymentType { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        public ICollection<OrderProduct> OrderProduct { get; set; }


    }
}
