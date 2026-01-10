using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Domain.Builder;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Interfaces;
using WebApplication1.Infrastructure.BackgroundTasks;
using WebApplication1.Infrastructure.BackgroundTasks.Interfaces;
using WebApplication1.Infrastructure.BackgroundTasks.Workers;
using WebApplication1.Infrastructure.Persistence;
using WebApplication1.Infrastructure.Persistence.Repository;
using WebApplication1.Infrastructure.Persistence.UnitOfWork;
using WebApplication1.Infrastructure.Sorting;
using WebApplication1.QueryHandler;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;

namespace Api.Extensions
{
    public static class AppServiceExtension
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies().UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });
            services.AddSingleton<IBackgroundTaskQueue>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<BackgroundTaskQueue>>();
                return new BackgroundTaskQueue(logger);
            });
            services.AddTransient<MovieQueryHandler>();
            services.AddTransient<QueryServices<TvSeries>>();
            services.AddTransient<QueryServices<Movie>>();
            services.AddTransient<QueryServices<Game>>();
            services.AddScoped<SorterContext<TvSeries>>();
            services.AddScoped<SorterContext<Movie>>();
            services.AddScoped<SorterContext<Game>>();
            services.AddScoped<IReferenceDataService, ReferenceDataService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddHostedService<QueueProcessorService>();
            services.AddHostedService<TokenBackgroundService>();
            services.AddScoped<AuthService>();
            services.AddHttpClient<LogSenderService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IMovieBuilder, MovieBuilder>();
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
