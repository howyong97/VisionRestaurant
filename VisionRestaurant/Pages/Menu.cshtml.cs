using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VisionRestaurant.Data;
using VisionRestaurant.Model;
using Microsoft.AspNetCore.Identity;

namespace VisionRestaurant.Pages
{
    public class MenuModel : PageModel
    {
        private readonly VisionRestaurant.Data.VisionRestaurantContext _context;
        private readonly VisionRestaurant.Data.VisionRestaurantContext _db;

        private readonly UserManager<ApplicationUser> _userManager;

        public MenuModel(VisionRestaurant.Data.VisionRestaurantContext context,VisionRestaurantContext db,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _db = db;
            _userManager = userManager;

        }

        public IList<Food> Food { get; set; } = default!;
        public async Task OnGetAsync()
        {
            if (_context.Food != null)
            {
                Food = await _context.Food.ToListAsync();
            }
        }
        public async Task<IActionResult> OnPostBuyAsync(int itemID)
        {
            var user = await _userManager.GetUserAsync(User);
            CheckoutCustomer customer = await _db
            .CheckoutCustomers
            .FindAsync(user.Email);

            var item = _db.BasketItems.FromSqlRaw("SELECT * FROM BasketItems WHERE ID = {0}" + " AND BasketID = {1}", itemID, customer.BasketID)
                        .ToList()
                        .FirstOrDefault();

            if (item == null)
            {
                BasketItem newItem = new BasketItem
                {
                    BasketID = customer.BasketID,
                    ID = itemID,
                    Quantity = 1
                };
                _db.BasketItems.Add(newItem);
                await _db.SaveChangesAsync();
            }
            else
            {
                item.Quantity = item.Quantity + 1;
                _db.Attach(item).State = EntityState.Modified;
                try
                {
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    throw new Exception($"Basket not found!", e);
                }
            }
            return RedirectToPage();



        }

    }
}
