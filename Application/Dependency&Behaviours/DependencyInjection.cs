using Application.Behaviours;
using Application.Dependency_Behaviours.Behaviours;
using Application.Features.Genres.GenreManager;
using Application.Features.Genres.GetAll;
using Application.Features.Genres.GetAllForMedias;
using Application.Features.Genres.GetAllForMovies;
using Domain.Aggregate;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
        {
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ErrorHandlingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehaviour<,>));
                cfg.AddOpenBehavior(typeof(CachingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(SendNotificationBehaviour<,>));
            });
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            services.AddMemoryCache();
            services.RegisterAllTypes(typeof(IGenreManager).Assembly);
            services.AddTransient<
                        IRequestHandler<GetUsedForMediaQuery<Game>, List<GenreResponse>>,
                        GetUsedForMediaHandler<Game>>();
            services.AddTransient<
                        IRequestHandler<GetUsedForMediaQuery<Movie>, List<GenreResponse>>,
                        GetUsedForMediaHandler<Movie>>();
            services.AddTransient<
                        IRequestHandler<GetUsedForMediaQuery<TvSeries>, List<GenreResponse>>,
                        GetUsedForMediaHandler<TvSeries>>();
            return services;
        }
    }
}
