using System.Text.Json.Serialization;
using API.Policies;
using API.Services;
using Application.Common.Interfaces.Authentication;

namespace API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddScoped<IUser, CurrentUser>();

            // Makes sure that you don't have to add foreignkey objects in de JSON
            services.AddControllers(options =>
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true
                ).AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = new LowerCaseNamingPolicy();
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddCors(options =>
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

            return services;
        }


        // TODO: Preparation for Azure keyvault
        /*
        public static IServiceCollection AddKeyVaultIfConfigured(this IServiceCollection services, ConfigurationManager configuration)
        {
            var keyVaultUri = configuration["AZURE_KEY_VAULT_ENDPOINT"];
            if (!string.IsNullOrWhiteSpace(keyVaultUri))
            {
                configuration.AddAzureKeyVault(
                    new Uri(keyVaultUri),
                    new DefaultAzureCredential());
            }

            return services;
        }
        */
    }
}
