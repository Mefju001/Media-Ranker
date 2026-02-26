using Application.Common.Interfaces;
using Application.Common.UserContext;
using Application.Features.AuthServices.Common;
using Application.Features.GamesServices.GetGamesByCriteria;
using Application.Features.MovieServices.GetAll;
using Application.Features.MovieServices.GetMoviesByCriteria;
using Application.Features.TvSeriesServices.GetTvSeriesByCriteria;
using Domain.DomainServices;
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
            services.AddScoped<IDirectorRepository, DirectorRepository>();
            services.AddScoped<ITvSeriesRepository, TvSeriesRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<ILikedMediaRepository, LikedMediaRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IMediaRepository, MediaRepository>();
            services.AddScoped<IUserPasswordService, UserPasswordService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ITokenCleanService, TokenCleanService>();
            services.AddScoped<IReferenceDataService, ReferenceDataService>();
            services.AddScoped<ITokenRepository, TokenRepository>();
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
