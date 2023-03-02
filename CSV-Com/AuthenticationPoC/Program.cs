using AuthenticationPoC.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using AuthenticationPoC.IdentityPolicy;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
var identityOptions = builder.Configuration.GetSection(nameof(IdentityOptions));
var serverVersion = MySqlServerVersion.LatestSupportedServerVersion;

// MSSQL --> MySQL
//var serverVersion = ServerVersion.AutoDetect(connectionString);
//builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(connectionString));

// Custom policy variables NOTE: Apparently custompolicies have to be added before calling add dbcontext. Else errors are shown double.
builder.Services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordPolicy>();
builder.Services.AddTransient<IUserValidator<AppUser>, CustomUsernameEmailPolicy>();

builder.Services.AddDbContext<AppIdentityDbContext>(
            dbContextOptions => dbContextOptions
                .UseMySql(connectionString, serverVersion)
                // The following three options help with debugging, but should
                // be changed or removed for production.
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

// Policy variables
builder.Services.Configure<IdentityOptions>(identityOptions);

// Custom policies
builder.Services.AddAuthorization(opts => {
    opts.AddPolicy("NederlandseAdmin", policy => {
        policy.RequireRole("Admin");
        policy.RequireClaim("Nationaliteit", "Nederlands");
    });
});

// Login URL
builder.Services.ConfigureApplicationCookie(opts => opts.LoginPath = "/Account/Login");

// Cookie settings for login
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = ".AspNetCore.Identity.Application";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.SlidingExpiration = true;
});

// Access denied. When authorization for a page failes. Default = /Account/AccessDenied
builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.AccessDeniedPath = "/Account/AccessDenied";
});

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
