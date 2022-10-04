using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using partner_aluro.DAL;
using partner_aluro.Models;
using partner_aluro.Services;
using partner_aluro.Services.Interfaces;
using System.Security.Policy;
using partner_aluro.Core;
using partner_aluro.Core.Repositories;
using partner_aluro.Repositories;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DbTestPracaContextConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    //options.IdleTimeout = TimeSpan.FromSeconds(60);
});

builder.Services.AddResponseCaching(x => x.MaximumBodySize = 1024); //1. dodatkowo do lwykorzystawiania cache


builder.Services.AddDbContext<ApplicationDbContext>(builder =>
{
    //builder.UseSqlServer(@"Data Source=mssql4.webio.pl,2401;Database=siwy55p_siwy55p;Uid=siwy55p_siwy55p;Password=Siiwy1a2!3!4!5!;TrustServerCertificate=True"); //connection string
    builder.UseSqlServer(connectionString); //connection string localbase

});

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

#region

AddAuthorizationPolicies();

#endregion

AddScoped();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.MapControllerRoute(
    name: "StronyStatyczne",
    pattern: "strony/{nazwa}",
    defaults: new { controller = "Home", action = "StronyStatyczne" });

//app.MapControllerRoute(
//    name: "ProduktyList",
//    pattern: "Kategoria/{nazwaKategori}",
//    defaults: new { controller = "Category", action = "Lista" });


//Endpoint
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.UseResponseCaching(); //2. dodatkowo do lwykorzystawiania cache

//app.Use(async (context, next) =>
//{
//    context.Response.GetTypedHeaders().CacheControl =
//    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
//    {
//        Public = true,
//        MaxAge = TimeSpan.FromSeconds(10)
//    };
//    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] = new string[] { "Accept-Encoding" };
//await next();
//}); //CACHE

app.Run();



void AddAuthorizationPolicies()
{
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("EmployeeNumber"));
    });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(Constants.Policies.RequireAdmin, policy => policy.RequireRole(Constants.Roles.Administrator));
        options.AddPolicy(Constants.Policies.RequireManager, policy => policy.RequireRole(Constants.Roles.Manager));
    });
}

void AddScoped()
{
    builder.Services.AddScoped<Cart>(sp => Cart.GetCart(sp));

    builder.Services.AddScoped<IProductService, ProductService>(); //Ktorej implementacji ma uzywac IWarehauseService
    builder.Services.AddScoped<ICategoryBD, CategoryBD>(); //Ktorej implementacji ma uzywac IWarehauseServic
    builder.Services.AddScoped<IApiService, ApiService>();
    builder.Services.AddScoped<IUnitOfWorkCategory, UnitOfWorkCategory>();
    builder.Services.AddScoped<IUnitOfWorkProduct, UnitOfWorkProduct>();

    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IRoleRepository, RoleRepository>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
}

#region

#endregion
