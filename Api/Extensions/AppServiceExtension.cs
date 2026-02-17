using Application.Common.Interfaces;
using Application.Features.AuthServices.Common;
using Application.Features.AuthServices.RefreshAccessToken;
using Application.Features.GamesServices.GetGamesByCriteria;
using Application.Features.MovieServices.GetAll;
using Application.Features.MovieServices.GetMoviesByCriteria;
using Application.Features.TvSeriesServices.GetTvSeriesByCriteria;
using Domain.Entity;
using FluentValidation;
using Infrastructure;
using Infrastructure.BackgroundTasks;
using Infrastructure.BackgroundTasks.CleanService;
using Infrastructure.BackgroundTasks.Workers;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repository;
using Infrastructure.Persistence.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions
{
    public static class AppServiceExtension
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<ITokenCleanService, TokenCleanService>();
            services.AddScoped<IReferenceDataService, ReferenceDataService>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<ITvSeriesSortAndFilterService, TvSeriesSortAndFilterService>();
            services.AddScoped<IMovieSortAndFilterService, MovieSortAndFilterService>();
            services.AddScoped<IGameSortAndFilterService, GameSortAndFilterService>();
            services.AddHostedService<TokenBackgroundService>();
            services.AddScoped<AccessTokenService>();
            services.AddScoped<MovieSortAndFilterService>();
            services.AddScoped<TvSeriesSortAndFilterService>();
            services.AddScoped<GameSortAndFilterService>();
            services.AddScoped<RefreshTokenService>();
            services.AddScoped<ValidatorForRefreshToken>();
            services.AddHttpClient<LogSenderService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(GetAllQuery).Assembly);
            });
            services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
