using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Services.Impl;

namespace WebApplication1.Extensions
{
    public static class AppServiceExtension
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies().UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });
               
            services.AddHostedService<TokenBackgroundService>();
            services.AddScoped<AuthService>();
            services.AddHttpClient<LogSenderService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IMovieServices, MovieServices>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IGameServices, GameServices>();
            services.AddScoped<IReviewServices, ReviewServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<ILikedMediaServices, LikedMediaServices>();
            services.AddScoped<ITvSeriesServices, TvSeriesServices>();
            services.AddScoped<ITokenCleanupService, TokenCleanupService>();

            return services;
        }
    }
}
