using WebApplication1.Models;
using WebApplication1.Strategy;
using WebApplication1.Strategy.Interfaces;

namespace WebApplication1.Extensions
{
    public static class GameSortTransient
    {
        public static IServiceCollection AddGameTransit(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddTransient<ISortingStrategy<Game>, DynamicSortingStrategy<Game>>(provider =>
                new DynamicSortingStrategy<Game>("title", g => g.title));
            services.AddTransient<ISortingStrategy<Game>,DynamicSortingStrategy<Game>>(provider=>
                new DynamicSortingStrategy<Game>("genre",g=>g.genre.name));
            services.AddTransient<ISortingStrategy<Game>, DynamicSortingStrategy<Game>>(provider=>
                new DynamicSortingStrategy<Game>("releaseDate",g=>g.ReleaseDate));
            services.AddTransient<ISortingStrategy<Game>, DynamicSortingStrategy<Game>>(provider =>
                new DynamicSortingStrategy<Game>("avarage", g => g.Reviews.Average(x=>(double?)x.Rating)??0));
            return services;
        }
    }
}
