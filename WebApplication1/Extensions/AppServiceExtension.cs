using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Builder;
using WebApplication1.Builder.Interfaces;
using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Observer;
using WebApplication1.Services;

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
            services.AddScoped<IMovieBuilder,MovieBuilder>();
            services.AddScoped<IGameBuilder, GameBuilder>();
            services.AddScoped<ITvSeriesBuilder, TvSeriesBuilder>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ApiLogSenderObserver).Assembly));
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
