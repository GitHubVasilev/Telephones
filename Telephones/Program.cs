using Telephones.API.Client.Interfaces;
using Telephones.API.Client.ClientAPI;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddAuthentication(config =>
        {
            config.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            config.DefaultChallengeScheme = "oidc";
        })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect("oidc", conf =>
    {
        conf.Authority = "https://localhost:7134/";
        conf.ClientId = "client_id_web";
        conf.ClientSecret = "client_secret_web";
        conf.SaveTokens = true;

        conf.ResponseType = "code";
    });

builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddControllersWithViews();
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<ITelephoneBookClientAPI, TelephoneBookClientAPI>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

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
