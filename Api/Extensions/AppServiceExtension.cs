using Application.Common.Interfaces;
using Application.Features.AuthServices.Common;
using Application.Features.GamesServices.GetGamesByCriteria;
using Application.Features.MovieServices.GetMoviesByCriteria;
using Application.Features.TvSeriesServices.GetTvSeriesByCriteria;
using Domain.Builder;
using Domain.Entity;
using Domain.Interfaces;
using FluentValidation;
using Infrastructure;
using Infrastructure.BackgroundTasks;
using Infrastructure.BackgroundTasks.Interfaces;
using Infrastructure.BackgroundTasks.Workers;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repository;
using Infrastructure.Persistence.UnitOfWork;
using Infrastructure.Sorting;
using Infrastructure.Specification.BuildPredicate.Game;
using Infrastructure.Specification.BuildPredicate.Movie;
using Infrastructure.Specification.BuildPredicate.TvSeriesSpec;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

            services.AddScoped<SorterContext<TvSeriesDomain>>();
            services.AddScoped<SorterContext<GameDomain>>();
            services.AddScoped<IReferenceDataService, ReferenceDataService>();

            services.AddScoped(typeof(ISorterContext<>), typeof(SorterContext<>)); services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddScoped<IGameFilter, GameFilter>();
            services.AddScoped<IMovieFilter, MovieFilter>();
            services.AddScoped<ITvSeriesFilter, TvSeriesFilter>();
            services.AddScoped<IGameBuildPredicate, GameBuildPredicate>();
            services.AddScoped<IMovieBuildPredicate, MovieBuildPredicate>();
            services.AddScoped<ITvSeriesBuildPredicate, TvSeriesBuildPredicate>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddHostedService<QueueProcessorService>();
            services.AddHostedService<TokenBackgroundService>();
            services.AddScoped<AccessTokenService>();
            services.AddScoped<RefreshTokenService>();
            services.AddHttpClient<LogSenderService>();
            services.AddScoped<IPasswordHasher<UserDomain>, PasswordHasher<UserDomain>>();
            services.AddScoped<IMovieBuilder, MovieBuilder>();
            services.AddScoped<IGameBuilder, GameBuilder>();
            services.AddScoped<ITvSeriesBuilder, TvSeriesBuilder>();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Application.Features.MovieServices.GetAll.GetAllQuery).Assembly);
            });
            services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            //services.AddScoped<IMovieServices, MovieServices>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<IGameServices, GameServices>();
            //services.AddScoped<ILikedMediaServices, LikedMediaServices>();
            ///services.AddScoped<ITvSeriesServices, TvSeriesServices>();
           // services.AddScoped<Application.Common.Interfaces.ITokenCleanupService, TokenCleanupService>();
            //services.AddScoped<Application.Common.Interfaces.IStatsUpdateService, StatsUpdateService>();

            return services;
        }
    }
}
