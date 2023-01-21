using Microsoft.EntityFrameworkCore;
using Telephones.IdentityServer;
using Telephones.IdentityServer.Data;
using Telephones.IdentityServer.Entities;
using Telephones.IdentityServer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString))
    .AddIdentity<ApplicationUser, ApplicationRole>(conf =>
    {
        conf.Password.RequireDigit = false;
        conf.Password.RequireLowercase = false;
        conf.Password.RequireUppercase = false;
        conf.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(conf =>
{
    conf.LoginPath = "/Auth/Login";
    conf.LogoutPath = "/Auth/Logout";
});

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<ApplicationUser>()
    .AddJwtBearerClientAuthentication()
    .AddInMemoryClients(Configurations.GetClients())
    .AddInMemoryApiResources(Configurations.GetApiResorces())
    .AddInMemoryIdentityResources(Configurations.GetIdentityResorces())
    .AddProfileService<ProfileService>()
    .AddInMemoryApiScopes(Configurations.GetApiScopes())
    .AddDeveloperSigningCredential();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    DataInitializer.Init(scope.ServiceProvider);
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseIdentityServer();

app.UseEndpoints(endpoints => 
{
    endpoints.MapDefaultControllerRoute();
});


app.Run();
