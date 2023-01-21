using Telephones.API.Client.Interfaces;
using Telephones.API.Client.ClientAPI;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddAuthentication(config =>
        {
            config.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            config.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, conf =>
    {
        conf.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        conf.RequireHttpsMetadata = true;
        conf.UsePkce = true;
        conf.Authority = "https://localhost:7134/";
        conf.ClientId = "client_id_web";
        conf.ClientSecret = "client_secret_web";
        conf.SaveTokens = true;

        conf.ResponseType = "code";

        conf.Scope.Add("TelephonesAPI");
        conf.Scope.Add("offline_access");
        
        conf.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;
        conf.GetClaimsFromUserInfoEndpoint = true;
        conf.ClaimActions.MapJsonKey(ClaimTypes.Role, ClaimTypes.Role);
    });

builder.Services.AddAuthorization(conf => 
    {
        conf.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "User"));
        conf.AddPolicy("User", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
    });


builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddControllersWithViews();
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<ITelephoneBookClientAPI, TelephoneBookClientAPI>();

var app = builder.Build();


app.UseRouting();
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseCookiePolicy();

app.UseCors(cors => cors
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TelephoneBook}/{action=Index}");

app.MapControllerRoute(
    name: "index",
    pattern: "{controller=TelephoneBook}/{action=Create}/{id?}");

app.MapControllerRoute(
    name: "details",
    pattern: "{controller=TelephoneBook}/{action=Details}/{id?}");

app.MapControllerRoute(
    name: "update",
    pattern: "{controller=TelephoneBook}/{action=Update}/{id?}");

app.MapControllerRoute(
    name: "delete",
    pattern: "{controller=TelephoneBook}/{action=Delete}/{id?}");

app.MapBlazorHub();
app.MapRazorPages();

app.Run();
