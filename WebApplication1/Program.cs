using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.Identity.Data;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("WebDbContextConnection") ?? throw new InvalidOperationException("Connection string 'WebDbContextConnection' not found.");

builder.Services.AddDbContext<WebDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add the other database here
/*var managingDbContextConnectionString = builder.Configuration.GetConnectionString("ManagingDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ManagingDbContext' not found.");
builder.Services.AddDbContext<ManagingDbContext>(options =>
options.UseSqlServer(managingDbContextConnectionString));*/


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<WebDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Home/LoginRegister";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();





app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "logout",
        pattern: "account/logout",
        defaults: new { controller = "Account", action = "Logout" });

    endpoints.MapControllerRoute(
        name: "dashboard",
        pattern: "dashboard/{action=Index}/{id?}",
        defaults: new { controller = "Dashboard" });

    endpoints.MapControllerRoute(
        name: "members",
        pattern: "members/{action=Index}/{id?}",
        defaults: new { controller = "Members" });

    endpoints.MapControllerRoute(
        name: "resource",
        pattern: "resource/{action=Index}/{id?}",
        defaults: new { controller = "Resource" });

    endpoints.MapControllerRoute(
        name: "messages",
        pattern: "messages/{action=Index}/{id?}",
        defaults: new { controller = "Messages" });

    endpoints.MapControllerRoute(
        name: "classes",
        pattern: "classes/{action=Index}/{id?}",
        defaults: new { controller = "Classes" });

    endpoints.MapControllerRoute(
        name: "settings",
        pattern: "settings/{action=Index}/{id?}",
        defaults: new { controller = "Settings" });

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapRazorPages();
});

app.Run();
