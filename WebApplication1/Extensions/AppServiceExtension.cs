using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApplication1.BackgroundService;
using WebApplication1.BackgroundService.Interfaces;
using WebApplication1.BackgroundTasks.Service;
using WebApplication1.Builder;
using WebApplication1.Builder.Interfaces;
using WebApplication1.Data;
using WebApplication1.DTO.Validator;
using WebApplication1.Models;
using WebApplication1.Observer;
using WebApplication1.QueryHandler;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;
using WebApplication1.Specification;
using WebApplication1.Specification.Interfaces;
using WebApplication1.Strategy;

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
            services.AddSingleton<IBackgroundTaskQueue>(provider => {
                var logger = provider.GetRequiredService<ILogger<BackgroundTaskQueue>>();
                return new BackgroundTaskQueue(logger, 100);
            });
            services.AddTransient<MovieQueryHandler>();
            services.AddTransient<QueryServices<TvSeries>>();
            services.AddTransient<QueryServices<Movie>>();
            services.AddTransient<QueryServices<Game>>();
            services.AddScoped<SorterContext<TvSeries>>();
            services.AddScoped<SorterContext<Movie>>();
            services.AddScoped<SorterContext<Game>>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddHostedService<QueueProcessorService>();
            services.AddHostedService<TokenBackgroundService>();
            services.AddScoped<AuthService>();
            services.AddHttpClient<LogSenderService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IMovieBuilder,MovieBuilder>();
            services.AddScoped<IGameBuilder, GameBuilder>();
            services.AddScoped<ITvSeriesBuilder, TvSeriesBuilder>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped<IMovieServices, MovieServices>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IGameServices, GameServices>();
            services.AddScoped<IReviewServices, ReviewServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<ILikedMediaServices, LikedMediaServices>();
            services.AddScoped<ITvSeriesServices, TvSeriesServices>();
            services.AddScoped<ITokenCleanupService, TokenCleanupService>();
            services.AddScoped<IStatsUpdateService, StatsUpdateService>();

            return services;
        }
    }
}
