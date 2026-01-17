using Application.Common.Interfaces;
using Domain.Entity;
using Infrastructure.Sorting;

namespace Api.Extensions
{
    public static class MovieSortTransient
    {
        public static IServiceCollection AddMovieTransit(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<ISortingStrategy<MovieDomain>, DynamicSortingStrategy<MovieDomain>>(provider =>
                new DynamicSortingStrategy<MovieDomain>("title", m => m.Title));
            services.AddTransient<ISortingStrategy<MovieDomain>, DynamicSortingStrategy<MovieDomain>>(provider =>
                new DynamicSortingStrategy<MovieDomain>("genre", m => m.GenreId));
            services.AddTransient<ISortingStrategy<MovieDomain>, DynamicSortingStrategy<MovieDomain>>(provider =>
                 new DynamicSortingStrategy<MovieDomain>("releaseDate", m => m.ReleaseDate));
            services.AddTransient<ISortingStrategy<MovieDomain>, DynamicSortingStrategy<MovieDomain>>(provider =>
                  new DynamicSortingStrategy<MovieDomain>("average", m => m.Stats.AverageRating ?? 0));
            return services;
        }
    }
}
