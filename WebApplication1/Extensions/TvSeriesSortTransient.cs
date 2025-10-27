using WebApplication1.Models;
using WebApplication1.Strategy;
using WebApplication1.Strategy.Interfaces;

namespace WebApplication1.Extensions
{
    public static class TvSeriesSortTransient
    {
        public static IServiceCollection AddTvSeriesTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ISortingStrategy<TvSeries>, DynamicSortingStrategy<TvSeries>>(provider =>
                new DynamicSortingStrategy<TvSeries>("title", tv => tv.title));
            services.AddTransient<ISortingStrategy<TvSeries>, DynamicSortingStrategy<TvSeries>>(provider =>
                new DynamicSortingStrategy<TvSeries>("genre", tv => tv.genre.name));
            return services;
        }
    }
}
