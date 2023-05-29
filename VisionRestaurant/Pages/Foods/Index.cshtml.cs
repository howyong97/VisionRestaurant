using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VisionRestaurant.Data;
using VisionRestaurant.Model;

namespace VisionRestaurant.Pages.Foods
{
    public class IndexModel : PageModel
    {
        private readonly VisionRestaurant.Data.VisionRestaurantContext _context;

        public IndexModel(VisionRestaurant.Data.VisionRestaurantContext context)
        {
            _context = context;
        }

        public IList<Food> Food { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Food != null)
            {
                Food = await _context.Food.ToListAsync();
            }
        }
    }
}
