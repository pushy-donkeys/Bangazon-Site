using BangazonWebsite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWebsite.Models.ViewModels
{
    public class OrderProductViewModel
    {
        public IEnumerable<Product> Product { get; set; }

        public double totalSale { get; set; }

        public OrderProductViewModel(int? id, ApplicationDbContext context)
        {
            this.Product = (from o in context.OrderProduct
                           where o.OrderId == id
                           join p in context.Product
                           on o.ProductId equals p.ProductId
                           join pty in context.ProductType
                           on p.ProductTypeId equals pty.ProductTypeId
                           select p).ToList();
            totalSale = (from y in Product select y.Price).Sum();
        }
    }
}
