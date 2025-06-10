using System.Text.Json.Serialization;
using API.Policies;
using API.Services;
using Application.Common.Interfaces.Authentication;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace API
{
    public static class DependencyInjection
    {
        private const string SCHEME_NAME = "Bearer";
        private const string BEARER_FORMAT = "JWT";
        private const string API_VERSION = "v1";
        private const string API_TITLE = "CVS API";
        private const string CORS_POLICY_NAME = "CvsCustomCorsPolicy";
        private const string SECURITY_SCHEME_NAME = "Authorization";
        private const string SECURITY_SCHEME_DESCRIPTION = "Please enter a valid token";

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
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(API_VERSION, new OpenApiInfo { Title = API_TITLE, Version = API_VERSION });
                options.AddSecurityDefinition(SCHEME_NAME,
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = SECURITY_SCHEME_DESCRIPTION,
                    Name = SECURITY_SCHEME_NAME,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = BEARER_FORMAT,
                    Scheme = SCHEME_NAME
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id=SCHEME_NAME
                            }
                        },
                        new string[]{}
                    }
                });
            });

            // JWT Authentication Configuration
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = BearerTokenService.GetTokenValidationParameters();
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: CORS_POLICY_NAME,
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
