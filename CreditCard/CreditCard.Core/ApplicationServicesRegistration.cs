using System.Reflection;
using CreditCard.Core.Application;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CreditCard.Core
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureCoreServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            return services;
        }
    }
}
