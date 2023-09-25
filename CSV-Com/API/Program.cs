using API.Policies;
using Application;
using Application.Common.Interfaces.CVS;
using Infrastructure.Persistence.Authentication;
using Infrastructure.Persistence.CVS;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Makes sure that you don't have to add foreignkey objects in de JSON
builder.Services.AddControllers(options =>
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true
    ).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = new LowerCaseNamingPolicy();
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CvsCustomCorsPolicy",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        // TODO: Uncomment for authentication
        //var initialiserAuthentication = scope.ServiceProvider.GetRequiredService<AuthenticationDbContextInitialiser>();        
        //await initialiserAuthentication.InitialiseAsync();
        //await initialiserAuthentication.SeedAsync();

        var initialiserCVS = scope.ServiceProvider.GetRequiredService<CVSDbContextInitialiser>();
        await initialiserCVS.InitialiseAsync();
        await initialiserCVS.SeedAsync();
    }    
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("CvsCustomCorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }