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

// Custom policy variables NOTE: Apperantly custompolicies have to be added before calling add dbcontext. Else errors are shown double.
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

// Login URL
builder.Services.ConfigureApplicationCookie(opts => opts.LoginPath = "/Authenticate/Login");

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
