using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BangazonWebsite.Data;


namespace BangazonWebsite.Models.ViewModels
{
    public class ProductTypeIndexViewModel
    {
        public ProductType ProductType { get; set; }
        public IEnumerable<object> TypesList { get; set; }


    public ProductTypeIndexViewModel() { }
    public ProductTypeIndexViewModel(ApplicationDbContext context) {

            

            this.TypesList = (from t in context.ProductType
                               join p in context.Product
                               on t.ProductTypeId equals p.ProductTypeId
                               group new { t, p } by new { t.Label } into grouped
                               select new
                               {
                                   TypeName = grouped.Key.Label,
                                   ProductCount = grouped.Select(x => x.p.ProductId).Count(),
                                   First3Products = grouped.Select(x => x.p.Title).Take(3)
                               }).ToList();
        }
    }
}
