using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    internal static class DbConnectExtensions
    {
        public static void AddDbConnect(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        }
    }
}
