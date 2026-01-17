using Application.Common.Interfaces;
using Domain.Entity;
using Infrastructure.Sorting;

namespace Api.Extensions
{
    public static class TvSeriesSortTransient
    {
        public static IServiceCollection AddTvSeriesTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ISortingStrategy<TvSeriesDomain>, DynamicSortingStrategy<TvSeriesDomain>>(provider =>
                new DynamicSortingStrategy<TvSeriesDomain>("title", tv => tv.Title));
            services.AddTransient<ISortingStrategy<TvSeriesDomain>, DynamicSortingStrategy<TvSeriesDomain>>(provider =>
                new DynamicSortingStrategy<TvSeriesDomain>("genre", tv => tv.GenreId));
            services.AddTransient<ISortingStrategy<TvSeriesDomain>, DynamicSortingStrategy<TvSeriesDomain>>(provider =>
                new DynamicSortingStrategy<TvSeriesDomain>("releaseDate", tv => tv.ReleaseDate));
            services.AddTransient<ISortingStrategy<TvSeriesDomain>, DynamicSortingStrategy<TvSeriesDomain>>(provider =>
                 new DynamicSortingStrategy<TvSeriesDomain>("avarage", tv => tv.Stats.AverageRating ?? 0));
            return services;
        }
    }
}
