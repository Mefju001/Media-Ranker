using Api.Extensions;

namespace Api
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityServices(config);
            services.AddSwaggerConfiguration(config);
            return services;
        }
    }
}
