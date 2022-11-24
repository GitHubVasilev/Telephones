using Microsoft.EntityFrameworkCore;
using Telephones.Data;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseMigrationsEndPoint();

using (var scope = app.Services.CreateScope())
{
    await DataInitializer.InitializerAsync(scope.ServiceProvider);
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TelephoneBook}/{action=Index}");

app.MapControllerRoute(
    name: "TelephoneBook",
    pattern: "{controller=TelephoneBook}/{action=Create}/{id?}");

app.MapControllerRoute(
    name: "TelephoneBook",
    pattern: "{controller=TelephoneBook}/{action=Details}/{id?}");

app.MapControllerRoute(
    name: "TelephoneBook",
    pattern: "{controller=TelephoneBook}/{action=Update}/{id?}");

app.MapControllerRoute(
    name: "TelephoneBook",
    pattern: "{controller=TelephoneBook}/{action=Delete}/{id?}");

app.Run();
