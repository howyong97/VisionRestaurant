using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using VisionRestaurant.Data;
using VisionRestaurant.Model;

namespace VisionRestaurant.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly VisionRestaurantContext _db;
        private readonly UserManager<ApplicationUser> _UserManager;
        public IList<CheckoutItems> Items { get; private set; }
        public OrderHistory Order = new OrderHistory();

        public decimal Total = 0;
        public long AmountPayable = 0;

        public CheckoutModel(VisionRestaurantContext db,UserManager<ApplicationUser> UserManager)
        {
            _db = db;
            _UserManager = UserManager;
        }

        public async Task OnGetAsync()
        {
            var user = await _UserManager.GetUserAsync(User);
            CheckoutCustomer customer = await _db
            .CheckoutCustomers
            .FindAsync(user.Email);

            Items = _db.CheckoutItems.FromSqlRaw(
                "SELECT Food.ID, Food.Price, " +
                "Food.Name, " +
                "BasketItems.BasketID, BasketItems.Quantity " +
                "FROM Food INNER JOIN BasketItems " +
                "ON Food.ID = BasketItems.ID " +
                "WHERE BasketID = {0}", customer.BasketID
                ).ToList();
            //Total = 0;
            //foreach (var item in Items)
            //{
                //Total += (item.Quantity * item.Price);
            //}
            //AmountPayable = (long)(Total * 100);

        }
        public async Task<IActionResult> OnPostBuyAsync()
        {
            var currentOrder = _db.OrderHistories.FromSqlRaw("SELECT * From OrderHistories")
                .OrderByDescending(b => b.OrderNo)
                .FirstOrDefault();

            if (currentOrder == null)
            {
                Order.OrderNo = 1;
            }
            else
            {
                Order.OrderNo = currentOrder.OrderNo + 1;
            }

            var user = await _UserManager.GetUserAsync(User);
            Order.Email = user.Email;
            _db.OrderHistories.Add(Order);

            CheckoutCustomer customer = await _db
                .CheckoutCustomers
                .FindAsync(user.Email);

            var basketItems =
                _db.BasketItems
                .FromSqlRaw("SELECT * From BasketItems " +
                "WHERE BasketID = {0}", customer.BasketID)
                .ToList();

            foreach (var item in basketItems)
            {
                OrderItem oi = new OrderItem
                {
                    OrderNo = Order.OrderNo,
                    ID = item.ID,
                    Quantity = item.Quantity
                };
                _db.OrderItems.Add(oi);
                _db.BasketItems.Remove(item);
            }

            await _db.SaveChangesAsync();

            return RedirectToPage("/Index");
        }


    }
}
