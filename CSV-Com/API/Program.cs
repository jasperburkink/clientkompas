using API;
using Application;
using Infrastructure;
using Infrastructure.Data.Authentication;
using Infrastructure.Data.CVS;

var builder = WebApplication.CreateBuilder(args);

System.Diagnostics.Debug.WriteLine("Loading settings for environment: " + builder.Environment.EnvironmentName);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

builder.Services.AddApiServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || true) // <- TODO: for now we want this scope to be executed for the test and dev environment as well, later we want to conditionally create and/or fill the database with test data.
{
    app.UseDeveloperExceptionPage();

    // Initialise and seed database
    using var scope = app.Services.CreateScope();

    var initialiserAuthentication = scope.ServiceProvider.GetRequiredService<AuthenticationDbContextInitialiser>();
    await initialiserAuthentication.InitialiseAsync();
    await initialiserAuthentication.SeedAsync();

    var initialiserCVS = scope.ServiceProvider.GetRequiredService<CVSDbContextInitialiser>();
    await initialiserCVS.InitialiseAsync();
    await initialiserCVS.SeedAsync();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("CvsCustomCorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
