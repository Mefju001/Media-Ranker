using Application.Common.Interfaces;
using Application.Common.UserContext;
using Application.Features.AuthServices.Common;
using Application.Features.GamesServices.GetGamesByCriteria;
using Application.Features.MovieServices.GetAll;
using Application.Features.MovieServices.GetMoviesByCriteria;
using Application.Features.TvSeriesServices.GetTvSeriesByCriteria;
using Domain.Aggregate;
using Domain.Repository;
using FluentValidation;
using Infrastructure.BackgroundTasks;
using Infrastructure.BackgroundTasks.CleanService;
using Infrastructure.BackgroundTasks.Workers;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repository;
using Infrastructure.Persistence.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IDirectorRepository, DirectorRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<ILikedMediaRepository, LikedMediaRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IMediaRepository, MediaRepository>();
            services.AddScoped<ITokenCleanService, TokenCleanService>();
            services.AddScoped<IReferenceDataService, ReferenceDataService>();
            services.AddScoped<ICurrentUserContext, CurrentUserContext>();
            services.AddScoped<ITvSeriesSortAndFilterService, TvSeriesSortAndFilterService>();
            services.AddScoped<IMovieSortAndFilterService, MovieSortAndFilterService>();
            services.AddScoped<IGameSortAndFilterService, GameSortAndFilterService>();
            services.AddHostedService<TokenBackgroundService>();
            services.AddScoped<MovieSortAndFilterService>();
            services.AddScoped<TvSeriesSortAndFilterService>();
            services.AddScoped<GameSortAndFilterService>();
            services.AddScoped<TokenServices>();
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
