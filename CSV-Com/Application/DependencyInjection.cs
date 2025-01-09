using System.Globalization;
using System.Reflection;
using Application.Common.Behaviours;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Common.Resources;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddAutoMapper(assembly);
            services.AddValidatorsFromAssembly(assembly);
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            });

            LoadResources(services, assembly);

            return services;
        }

        private static void LoadResources(IServiceCollection services, Assembly assembly)
        {
            var resourceProvider = new ResourceMessageProvider(CultureInfo.CurrentUICulture); // TODO: Should load resources for all available cultures in de application.

            foreach (var resourceType in ResourceHelper.GetResourceTypes(assembly))
            {
                resourceProvider.LoadResources(resourceType);
            }

            services.AddSingleton<IResourceMessageProvider>(resourceProvider);
        }
    }
}
