using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Domain.Entities;
using WebApplication1.Infrastructure.Sorting;

namespace Api.Extensions
{
    public static class TvSeriesSortTransient
    {
        public static IServiceCollection AddTvSeriesTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ISortingStrategy<TvSeries>, DynamicSortingStrategy<TvSeries>>(provider =>
                new DynamicSortingStrategy<TvSeries>("title", tv => tv.title));
            services.AddTransient<ISortingStrategy<TvSeries>, DynamicSortingStrategy<TvSeries>>(provider =>
                new DynamicSortingStrategy<TvSeries>("genre", tv => tv.genre.name));
            services.AddTransient<ISortingStrategy<TvSeries>, DynamicSortingStrategy<TvSeries>>(provider =>
                new DynamicSortingStrategy<TvSeries>("releaseDate", tv => tv.ReleaseDate));
            services.AddTransient<ISortingStrategy<TvSeries>, DynamicSortingStrategy<TvSeries>>(provider =>
                 new DynamicSortingStrategy<TvSeries>("avarage", tv => tv.Stats.AverageRating ?? 0));
            return services;
        }
    }
}
