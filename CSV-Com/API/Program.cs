using Application.Common.Interfaces.CVS;
using Infrastructure.Persistence.CVS;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionStringAuthentication = builder.Configuration.GetValue<string>("ConnectionStrings:AuthenticationConnectionString");
var connectionStringCVS = builder.Configuration.GetValue<string>("ConnectionStrings:CVSConnectionString");

var serverVersion = MySqlServerVersion.LatestSupportedServerVersion;

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

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
