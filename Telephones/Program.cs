using Microsoft.EntityFrameworkCore;
using Telephones.API.Client.Interfaces;
using Telephones.API.Client.ClientAPI;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddControllersWithViews();
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<ITelephoneBookClientAPI, TelephoneBookClientAPI>();

var app = builder.Build();

app.UseMigrationsEndPoint();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapBlazorHub();
app.MapRazorPages();

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
