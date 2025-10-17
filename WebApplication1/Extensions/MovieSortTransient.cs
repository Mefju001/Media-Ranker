using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Strategy;
using WebApplication1.Strategy.Interfaces;

namespace WebApplication1.Extensions
{
    public static class MovieSortTransient
    {
        public static IServiceCollection AddMovieTransit(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<MovieQueryHandler>();
            services.AddScoped<MovieSorterContext>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<ISortingStrategy<Movie>, DynamicSortingStrategy<Movie>>(provider =>
                new DynamicSortingStrategy<Movie>("titleascending",m => m.title, false)); // false = ASC

            // Strategia B: Sortowanie po Ocenie (malejąco)
            /*services.AddTransient<ISortingStrategy<Movie>, DynamicSortingStrategy<Movie>>(provider =>
                new DynamicSortingStrategy<Movie>(m => m., true)); // true = DESC*/

            // Strategia C: Sortowanie po Dacie (rosnąco)
            services.AddTransient<ISortingStrategy<Movie>, DynamicSortingStrategy<Movie>>(provider =>
                new DynamicSortingStrategy<Movie>("releaseascending", m => m.ReleaseDate, false));
            return services;
        }
    }
}
