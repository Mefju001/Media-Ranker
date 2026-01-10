using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Domain.Entities;
using WebApplication1.Infrastructure.Sorting;

namespace Api.Extensions
{
    public static class GameSortTransient
    {
        public static IServiceCollection AddGameTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ISortingStrategy<Game>, DynamicSortingStrategy<Game>>(provider =>
                new DynamicSortingStrategy<Game>("title", g => g.title));
            services.AddTransient<ISortingStrategy<Game>, DynamicSortingStrategy<Game>>(provider =>
                new DynamicSortingStrategy<Game>("genre", g => g.genre.name));
            services.AddTransient<ISortingStrategy<Game>, DynamicSortingStrategy<Game>>(provider =>
                new DynamicSortingStrategy<Game>("releaseDate", g => g.ReleaseDate));
            services.AddTransient<ISortingStrategy<Game>, DynamicSortingStrategy<Game>>(provider =>
                new DynamicSortingStrategy<Game>("average", g => g.Stats.AverageRating ?? 0));
            return services;
        }
    }
}
