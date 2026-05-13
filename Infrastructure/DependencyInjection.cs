using Application.Behaviours;
using Infrastructure.Database.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbConnect(config);
            services.AddServices();
            services.RegisterAllTypes(typeof(DirectorRepository).Assembly);
            return services;
        }
    }
}
