using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using Verkoop2025Data.Repositories;
using Verkoop2025Services;
using Verkoop2025Web;
using Verkoop2025Web.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PrulariaComContext>(
        options => options.UseMySQL(
         builder.Configuration.GetConnectionString("PrulariaComConnection")!,
                           x => x.MigrationsAssembly("Verkoop2025Data")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new LoginActionFilter());
});
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo> {new CultureInfo("nl-BE"),};
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.DefaultRequestCulture = new RequestCulture("nl-BE");

});

builder.Services.AddLocalization();
builder.Services.AddTransient<IOrderRepository, SQLOrderRepository>();
builder.Services.AddTransient<ICustomerRepository, SQLCustomerRepository>();
builder.Services.AddTransient<IAccountRepository, SQLAccountRepository>();
builder.Services.AddTransient<OrderService>();
builder.Services.AddTransient<AccountService>();
builder.Services.AddTransient<CustomerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseRequestLocalization();
app.UseHttpsRedirection();
app.UseSession();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Client}/{action=Login}/{id?}")
    .WithStaticAssets();

app.Run();