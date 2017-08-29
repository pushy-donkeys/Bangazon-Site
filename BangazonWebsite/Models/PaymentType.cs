using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWebsite.Models
{
    public class PaymentType
	{
        [Key]
        public int PaymentTypeId { get; set; }
    
        [Required]
        [StringLength(12)]
        public string Description { get; set; }

        [Required]
        [StringLength(20)]
        public string AccountNumber { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }

    }
}
