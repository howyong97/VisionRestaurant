using System;
using System.Collections.Generic;
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

        //public DbSet<ImageModel> Image { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
