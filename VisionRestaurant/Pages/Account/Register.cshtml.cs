using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisionRestaurant.Data;
using VisionRestaurant.Model;

namespace VisionRestaurant.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegistrationModel Input { get; set; }
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;


        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                //check that there is an Adminsitrator role and create if not
                Task<bool> hasRegUserRole = _roleManager.RoleExistsAsync("RegisteredUser");
                hasRegUserRole.Wait();

                if (!hasRegUserRole.Result)
                {
                    var roleResult = _roleManager.CreateAsync(new IdentityRole("RegisteredUser"));
                    roleResult.Wait();
                }

                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    Task<IdentityResult> newUserRole
                        = _userManager.AddToRoleAsync(user, "RegisteredUser");
                    newUserRole.Wait();
                    return RedirectToPage("/Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();


        }
    }
}