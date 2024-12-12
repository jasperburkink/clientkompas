using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmailModule
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddEmailModuleServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
