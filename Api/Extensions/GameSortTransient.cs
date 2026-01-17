using Application.Common.Interfaces;
using Domain.Entity;
using Infrastructure.Sorting;

namespace Api.Extensions
{
    public static class GameSortTransient
    {
        public static IServiceCollection AddGameTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ISortingStrategy<GameDomain>, DynamicSortingStrategy<GameDomain>>(provider =>
                new DynamicSortingStrategy<GameDomain>("title", g => g.Title));
            services.AddTransient<ISortingStrategy<GameDomain>, DynamicSortingStrategy<GameDomain>>(provider =>
                new DynamicSortingStrategy<GameDomain>("genre", g => g.GenreId));
            services.AddTransient<ISortingStrategy<GameDomain>, DynamicSortingStrategy<GameDomain>>(provider =>
                new DynamicSortingStrategy<GameDomain>("releaseDate", g => g.ReleaseDate));
            services.AddTransient<ISortingStrategy<GameDomain>, DynamicSortingStrategy<GameDomain>>(provider =>
                new DynamicSortingStrategy<GameDomain>("average", g => g.Stats.AverageRating ?? 0));
            return services;
        }
    }
}
