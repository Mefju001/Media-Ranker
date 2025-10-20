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
                new DynamicSortingStrategy<Movie>("title",m => m.title));
            services.AddTransient<ISortingStrategy<Movie>,DynamicSortingStrategy<Movie>>(provider =>
                new DynamicSortingStrategy<Movie>("genre",m=>m.genre.name));
            services.AddTransient<ISortingStrategy<Movie>, DynamicSortingStrategy<Movie>>(provider =>
                 new DynamicSortingStrategy<Movie>("releaseDate", m => m.ReleaseDate));
            return services;
        }
    }
}
