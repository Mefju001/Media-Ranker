using Application.Behaviours;
using Application.Features.Genres.GenreManager;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
        {
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ErrorHandlingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehaviour<,>));
                cfg.AddOpenBehavior(typeof(SendNotificationBehaviour<,>));
            });
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            services.RegisterAllTypes(typeof(IGenreManager).Assembly);
            return services;
        }
    }
}
