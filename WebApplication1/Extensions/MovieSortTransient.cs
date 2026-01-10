using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Domain.Entities;
using WebApplication1.Infrastructure.Sorting;

namespace Api.Extensions
{
    public static class MovieSortTransient
    {
        public static IServiceCollection AddMovieTransit(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<ISortingStrategy<Movie>, DynamicSortingStrategy<Movie>>(provider =>
                new DynamicSortingStrategy<Movie>("title", m => m.title));
            services.AddTransient<ISortingStrategy<Movie>, DynamicSortingStrategy<Movie>>(provider =>
                new DynamicSortingStrategy<Movie>("genre", m => m.genre.name));
            services.AddTransient<ISortingStrategy<Movie>, DynamicSortingStrategy<Movie>>(provider =>
                 new DynamicSortingStrategy<Movie>("releaseDate", m => m.ReleaseDate));
            services.AddTransient<ISortingStrategy<Movie>, DynamicSortingStrategy<Movie>>(provider =>
                  new DynamicSortingStrategy<Movie>("average", m => m.Stats.AverageRating ?? 0));
            return services;
        }
    }
}
