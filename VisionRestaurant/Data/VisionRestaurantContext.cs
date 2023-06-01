using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VisionRestaurant.Model;

namespace VisionRestaurant.Data
{
    public class VisionRestaurantContext : IdentityDbContext<ApplicationUser>
    {
        public VisionRestaurantContext (DbContextOptions<VisionRestaurantContext> options)
            : base(options)
        {
        }

        public DbSet<VisionRestaurant.Model.Food> Food { get; set; } = default!;
        public DbSet<CheckoutCustomer> CheckoutCustomers { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }

        [NotMapped]
        public DbSet<CheckoutItems> CheckoutItems { get; set; }

        public DbSet<OrderHistory> OrderHistories { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }



        //public DbSet<ImageModel> Image { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<BasketItem>().HasKey(t => new { t.ID, t.BasketID });
            builder.Entity<OrderItem>().HasKey(t => new { t.ID, t.OrderNo });
        }
    }
}
