using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VisionRestaurant.Data;
using VisionRestaurant.Model;
using Stripe;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
builder.Services.AddDbContext<VisionRestaurantContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VisionRestaurantContext") ?? throw new InvalidOperationException("Connection string 'VisionRestaurantContext' not found.")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<VisionRestaurantContext>()
    .AddRoles<IdentityRole>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = new PathString("/Account/Login");
    options.AccessDeniedPath = new PathString("/Account/AccessDenied");
    options.LogoutPath = new PathString("/Index");
    
});

//define the admin policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy",
        policy => policy.RequireRole("Administrator"));
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Foods", "AdminPolicy");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["sk_test_51NDSQtHDMFKa0vnJI7WQJkRtfNS6d1N522NHT9YpmZCLfNTt9ltfAVTtpcSkxr1yzOoKaptgetkOUnr6psHV5Ja800jKpNCj5h"];

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
