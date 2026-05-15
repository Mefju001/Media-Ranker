using Api.Extensions;
using Application.Features.Auth.Common;

namespace Api
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityServices(config);
            services.AddSwaggerConfiguration(config);
            services.Configure<JwtSettings>(config.GetSection("Jwt"));
            return services;
        }
    }
}
