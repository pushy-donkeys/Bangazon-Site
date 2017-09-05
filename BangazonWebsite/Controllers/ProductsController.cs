using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BangazonWebsite.Data;
using BangazonWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BangazonWebsite.Models.ViewModels;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace BangazonWebsite.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _environment;

        public ProductsController(ApplicationDbContext ctx, UserManager<ApplicationUser> userManager, IHostingEnvironment environment)
        {
            _userManager = userManager;
            _context = ctx;
            _environment = environment;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Products
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Product.Include(p => p.ProductType);
            return View(await applicationDbContext.ToListAsync());
        }

        //GET SEARCH PRODUCT
        [Authorize]
        public async Task<IActionResult> Search(string searchFor, string searchText)
        {
            ProductListViewModel viewModel = new ProductListViewModel();
           
            if(!String.IsNullOrEmpty(searchText) && searchFor.Equals("Product"))
            {
                viewModel.product = await _context.Product.Where(s => s.Title.ToLower().Contains(searchText.ToLower()) || s.Description.ToLower().Contains(searchText.ToLower())).ToListAsync();
            }
            else if(!String.IsNullOrEmpty(searchText) && searchFor.Equals("LocalDelivery"))
            {
                viewModel.product = await _context.Product.Where(l => l.LocalDelivery.Equals(true) && l.City.ToLower().Contains(searchText.ToLower())).ToListAsync();
            }

            return View(viewModel);
        }

        // GET: Products/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.ProductType)
                .SingleOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Details
        [HttpPost]
        public async Task<IActionResult> AddToOrder (int ProductId)
        {
            var model = new OrderViewModel();
            //Brings back the product we want to add
            //_context.Product loops through all products and finds the one (p.ProductId) that matches ProductId (the one selected to add)
           var prod = await _context.Product
                .SingleOrDefaultAsync(p => p.ProductId == ProductId);

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound();
            }
            //check for orders
            //first checking if user on order is the same as current user, and if PT is null (incomplete order)
            var currentOrders = await _context.Order
                .SingleOrDefaultAsync(m => m.User == user && m.PaymentTypeId == null);

            //add product to order
            //with SingleOrDEfault, if nothing comes back it is null
            if (currentOrders == null)
            {
                //create Order instance and pass in user to create order
                Order order = new Order() { User = user };
                _context.Order.Add(order);
                //create OrderProduct instance to make an OrderProduct
                OrderProduct op = new OrderProduct() { ProductId = prod.ProductId, OrderId = order.OrderId };
                _context.OrderProduct.Add(op);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Orders", new { id = order.OrderId });
            }
            else
            {
                //if there IS an order and you don't need to create one, add product to order
                OrderProduct op = new OrderProduct() { ProductId = prod.ProductId, OrderId = currentOrders.OrderId };
                _context.OrderProduct.Add(op);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Orders", new { id = currentOrders.OrderId });
        }

        // GET: Products/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {

            ProductCreateViewModel model = new ProductCreateViewModel(_context);
            model.Product = new Product();
            var user = await GetCurrentUserAsync();
            return View(model);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create( ProductCreateViewModel model)
        {
            ModelState.Remove("product.User");
            if (ModelState.IsValid)
            {
                long size = 0;
                foreach (var file in model.image)
                {
                    var filename = ContentDispositionHeaderValue
                                    .Parse(file.ContentDisposition)
                                    .FileName
                                    .Trim('"');
                    filename = _environment.WebRootPath + $@"\products\{file.FileName.Split('\\').Last()}";
                    size += file.Length;
                    using (var fileStream = new FileStream(filename, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        model.Product.ImgPath = $@"\products\{file.FileName.Split('\\').Last()}";
                    }
                }
                var user = await GetCurrentUserAsync();
                model.Product.User = user;
                _context.Add(model.Product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "ProductTypeId", "ProductTypeId", model.Product.ProductTypeId);
            ProductCreateViewModel model2 = new ProductCreateViewModel(_context);
            return View(model2);

        }

        public async Task<IActionResult> Types()
        {
            var model = new ProductTypeViewModel();

            // Get line items grouped by product id, including count
            var counter = from product in _context.Product
                          group product by product.ProductTypeId into grouped
                          select new { grouped.Key, myCount = grouped.Count() };

            // Build list of Product Type instances for display in view
            model.ProductTypes = await (from type in _context.ProductType
                                        join a in counter on type.ProductTypeId equals a.Key
                                        select new ProductType
                                        {
                                            ProductTypeId = type.ProductTypeId,
                                            Label = type.Label
                                        }).ToListAsync();

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
            // GET: Products/Delete/5
            public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.ProductType)
                .SingleOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.SingleOrDefaultAsync(m => m.ProductId == id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
