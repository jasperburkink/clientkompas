﻿using AuthenticationPoC.CustomPolicy;
using AuthenticationPoC.IdentityPolicy;
using AuthenticationPoC.Models;
using CVSInfrastructurePoC;
using CVSInfrastructurePoC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionStringAuthentication = builder.Configuration.GetValue<string>("ConnectionStrings:AuthenticationConnection");
var connectionStringCVS = builder.Configuration.GetValue<string>("ConnectionStrings:CVSConnection");

var serverVersion = MySqlServerVersion.LatestSupportedServerVersion;

// MSSQL --> MySQL
//var serverVersion = ServerVersion.AutoDetect(connectionString);

//builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(connectionStringAuthentication));

// Custom policy variables NOTE: Apparently custompolicies have to be added before calling add dbcontext. Else errors are shown double.
builder.Services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordPolicy>();
builder.Services.AddTransient<IUserValidator<AppUser>, CustomUsernameEmailPolicy>();

builder.Services.AddDbContext<CVSDbContext>(
dbContextOptions => dbContextOptions
                .UseMySql(connectionStringCVS, serverVersion,
                mySqlOptions =>
                {
                    mySqlOptions.MigrationsAssembly(typeof(CVSDbContext).Assembly.FullName); // Migrations in class library
                })
                // The following three options help with debugging, but should
                // be changed or removed for production.
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());

builder.Services.AddDbContext<AppIdentityDbContext>(
            dbContextOptions => dbContextOptions
                .UseMySql(connectionStringAuthentication, serverVersion)
                // The following three options help with debugging, but should
                // be changed or removed for production.
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

builder.Services.AddScoped<IGebruikerRepository, GebruikerRepository>();

// Policy variables
var identityOptions = builder.Configuration.GetSection(nameof(IdentityOptions));
builder.Services.Configure<IdentityOptions>(identityOptions);

//builder.Services.Configure<IdentityOptions>(opts => {
//    opts.User.RequireUniqueEmail = true;
//    opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
//    opts.Password.RequiredLength = 8;
//    opts.Password.RequireLowercase = true;
//});

// Custom policies static
builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("NederlandseAdmin", policy =>
    {
        policy.RequireRole("Admin");
        policy.RequireClaim("Nationaliteit", "Nederlands");
    });
});


// Custom user policy with custom user policy class
builder.Services.AddTransient<IAuthorizationHandler, AllowUsersHandler>();
builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("AllowJasper", policy =>
    {
        policy.AddRequirements(new AllowUserPolicy("Jasper"));
    });
});

// Custom user policy without constructor --> Can be used to add authorization without the authorization attribute
builder.Services.AddTransient<IAuthorizationHandler, AllowPrivateHandler>();
builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("PrivateAccess", policy =>
    {
        policy.AddRequirements(new AllowPrivatePolicy());
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
