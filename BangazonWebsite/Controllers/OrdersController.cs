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
using Microsoft.AspNetCore.Hosting;

namespace BangazonWebsite.Controllers
{
    public class OrdersController : Controller
    {
        //necessary for getting user id:
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _environment;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment environment)
        {
            _userManager = userManager;
            _environment = environment;
            _context = context;    
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Orders
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Order.Include(o => o.PaymentType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                //if there's no id, this will get user id and go to that order detail page without having /1
                var currentUser = await GetCurrentUserAsync();
                var currentOrder = await _context.Order
                 .Include(o => o.PaymentType)
                 .Include(o => o.OrderProduct)
                 .ThenInclude(o => o.Product)
                 // 'm' is each order in database, id is the one we are clicking
                 .SingleOrDefaultAsync(m => m.User == currentUser && m.PaymentType == null);

                if (currentOrder == null)
                {
                    return NotFound();
                }

                return View(currentOrder);
            }
            //include orderproduct and then include product to show them on shopping cart
            var order = await _context.Order
                .Include(o => o.PaymentType)
                .Include(o => o.OrderProduct)
                .ThenInclude(o => o.Product)
                // 'm' is each order in database, id is the one we are clicking
                .SingleOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["PaymentTypeId"] = new SelectList(_context.Set<PaymentType>(), "PaymentTypeId", "AccountNumber");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("OrderId,PaymentTypeId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["PaymentTypeId"] = new SelectList(_context.Set<PaymentType>(), "PaymentTypeId", "AccountNumber", order.PaymentTypeId);
            return View(order);
        }

        // GET: Orders/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.SingleOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["PaymentTypeId"] = new SelectList(_context.Set<PaymentType>(), "PaymentTypeId", "AccountNumber", order.PaymentTypeId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,PaymentTypeId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["PaymentTypeId"] = new SelectList(_context.Set<PaymentType>(), "PaymentTypeId", "AccountNumber", order.PaymentTypeId);
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.OrderProduct
                .SingleOrDefaultAsync(m => m.OrderProductId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderproduct = await _context.OrderProduct.SingleOrDefaultAsync(m => m.OrderProductId == id);
            _context.OrderProduct.Remove(orderproduct);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details");
        }

        //not on delete
        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }
    }
}
