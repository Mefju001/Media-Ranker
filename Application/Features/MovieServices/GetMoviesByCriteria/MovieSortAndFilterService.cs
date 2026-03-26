using Application.Common.Interfaces;
using Domain.Entity;
using System.Linq.Expressions;

namespace Application.Features.MovieServices.GetMoviesByCriteria
{
    public class MovieSortAndFilterService : IMovieSortAndFilterService
    {
        private readonly IMovieRepository movieRepository;
        private readonly IGenreRepository genreRepository;
        private readonly IDirectorRepository directorRepository;
        public MovieSortAndFilterService(IMovieRepository movieRepository, IGenreRepository genreRepository, IDirectorRepository directorRepository)
        {
            this.movieRepository = movieRepository;
            this.genreRepository = genreRepository;
            this.directorRepository = directorRepository;
        }
        public IQueryable<Movie> Handler(GetMoviesByCriteriaQuery request)
        {
            var filteredMovies = ApplyFilters(request);
            var sortedMovies = ApplySorting(filteredMovies, request);
            return sortedMovies;
        }
        private IQueryable<Movie> ApplyFilters(GetMoviesByCriteriaQuery request)
        {
            var query = movieRepository.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.TitleSearch))
            {
                query = query.Where(m => m.Title.Contains(request.TitleSearch));
            }
            if (!string.IsNullOrWhiteSpace(request.genreName))
            {
                var genreQuery = genreRepository.GetAllQueryable();
                query = query.Join(genreQuery,
                    movie => movie.GenreId,
                    genre => genre.Id,
                    (movie, genre) => new { Movie = movie, Genre = genre })
                    .Where(mg => mg.Genre.name.Value.Contains(request.genreName))
                    .Select(mg => mg.Movie);
            }
            if (request.MinRating.HasValue)
            {
                query = query.Where(m => m.Stats!.AverageRating >= request.MinRating);
            }
            if (request.ReleaseYear.HasValue)
            {
                query = query.Where(m => m.ReleaseDate!.Value.Year == request.ReleaseYear);
            }
            if (!string.IsNullOrWhiteSpace(request.DirectorSurname) && !string.IsNullOrWhiteSpace(request.DirectorSurname))
            {
                var directorQuery = directorRepository.GetAllQueryable();
                query = query.Join(
                    directorQuery,
                    movie => movie.DirectorId,
                    director => director.Id,
                    (movie, directorQuery) => new { Movie = movie, Director = directorQuery })
                    .Where(md => md.Director.name.Contains(request.DirectorName!) && md.Director.surname.Contains(request.DirectorSurname!))
                    .Select(md => md.Movie);
            }
            return query;
        }
        private IQueryable<Movie> ApplySorting(IQueryable<Movie> query, GetMoviesByCriteriaQuery request)
        {
            if (request.SortByField != null)
            {
                var strings = request.SortByField.Split("|");
                request.SortByField = strings[0];
                request.IsDescending = strings[1].ToLower().Equals("false") ? false : true;
            }
            if (!string.IsNullOrEmpty(request.SortByField) && sortColumns.TryGetValue(request.SortByField, out var sortExpression))
            {
                return request.IsDescending
                    ? query.OrderByDescending(sortExpression)
                    : query.OrderBy(sortExpression);
            }
            return query;
        }
        private static readonly Dictionary<string, Expression<Func<Movie, object>>> sortColumns =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["Title"] = m => m.Title,
                ["Rating"] = m => m.Stats.AverageRating!,
                ["Date"] = m => m.ReleaseDate!,
                ["Director"] = m => m.DirectorId
            };
    }
}
