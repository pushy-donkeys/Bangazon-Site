﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BangazonWebsite.Models;

namespace BangazonWebsite.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        public DbSet<BangazonWebsite.Models.ProductType> ProductType { get; set; }
        public DbSet<BangazonWebsite.Models.ApplicationUser> ApplicationUser { get; set; }
        public DbSet<BangazonWebsite.Models.Order> Order { get; set; }
        public DbSet<BangazonWebsite.Models.PaymentType> PaymentType { get; set; }
        public DbSet<BangazonWebsite.Models.Product> Product { get; set; }
        public DbSet<BangazonWebsite.Models.OrderProduct> OrderProduct { get; set; }

    }
}