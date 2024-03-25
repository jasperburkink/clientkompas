using System.Text.Json.Serialization;
using API.Policies;
using Infrastructure;
using Infrastructure.Persistence.CVS;

var builder = WebApplication.CreateBuilder(args);

System.Diagnostics.Debug.WriteLine("Loading settings for environment: " + builder.Environment.EnvironmentName);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

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
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
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
if (app.Environment.IsDevelopment() || true) // <- for now we want this scope to be executed for the test and dev environment as well
{
    app.UseDeveloperExceptionPage();

    // Initialise and seed database
    using var scope = app.Services.CreateScope();
    // TODO: Uncomment for authentication
    //var initialiserAuthentication = scope.ServiceProvider.GetRequiredService<AuthenticationDbContextInitialiser>();
    //await initialiserAuthentication.InitialiseAsync();
    //await initialiserAuthentication.SeedAsync();

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
