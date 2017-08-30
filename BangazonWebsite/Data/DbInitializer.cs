using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BangazonWebsite.Data;
using BangazonWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Bangazon.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any product types.
                if (context.ProductType.Any())
                {
                    return;   // DB has been seeded
                }

                var productTypes = new ProductType[]
                {
                    new ProductType {
                        Label = "Movies, Music, Games"
                    },
                    new ProductType {
                        Label = "Electronics, Computers & Office"
                    },
                     new ProductType {
                        Label = "Books"
                    },
                    new ProductType {
                        Label = "Clothing, Shoes & Jewelry"
                    },
                     new ProductType {
                        Label = "Kitchen&Dining"
                    },
                    new ProductType {
                        Label = "Patio&Garden"
                    },
                    new ProductType {
                        Label = "Toys, Kids, and Baby"
                    },
                    new ProductType {
                        Label = "Automotive and Industrial"
                    },
                    new ProductType {
                        Label = "Tools"
                    },
                    new ProductType {
                        Label = "Appliances"
                    },
                    new ProductType {
                        Label = "Home Goods"
                    },
                    new ProductType {
                        Label = "Furniture"
                    },
                    new ProductType {
                        Label = "Sports and Outdoor"
                    },
                    new ProductType {
                        Label = "Beauty"
                    },
                    new ProductType {
                        Label = "Health"
                    },
                    new ProductType {
                        Label = "Food&Beverage"
                    },
                    new ProductType {
                        Label = "Other"
                    }
                };

                foreach (ProductType i in productTypes)
                {
                    context.ProductType.Add(i);
                }
                context.SaveChanges();
            }
        }
    }
}
