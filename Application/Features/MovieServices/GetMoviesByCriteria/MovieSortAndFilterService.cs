using Application.Common.Interfaces;
using Domain.Entity;
using System.Linq.Expressions;

namespace Application.Features.MovieServices.GetMoviesByCriteria
{
    public class MovieSortAndFilterService: IMovieSortAndFilterService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MovieSortAndFilterService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IQueryable<Movie>> Handler(GetMoviesByCriteriaQuery request)
        {
            var query = ApplyFilters(request);
            query = ApplySorting(query, request);
            return query;
        }
        private IQueryable<Movie> ApplyFilters(GetMoviesByCriteriaQuery request)
        {
            var query = _unitOfWork.MovieRepository.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.TitleSearch))
            {
                query = query.Where(m => m.Title.Contains(request.TitleSearch));
            }
            if (!string.IsNullOrWhiteSpace(request.genreName))
            {
                var genreQuery = _unitOfWork.GenreRepository.GetAllQueryable();
                query = query.Join(genreQuery,
                    movie=>movie.GenreId,
                    genre=>genre.Id,
                    (movie ,genre)=> new {Movie=movie, Genre = genre })
                    .Where(mg=>mg.Genre.name.Value.Contains(request.genreName))
                    .Select(mg=>mg.Movie);
            }
            if (request.MinRating.HasValue)
            {
                query = query.Where(m => m.Stats!.AverageRating >= request.MinRating);
            }
            if (request.ReleaseYear.HasValue)
            {
                query = query.Where(m => m.ReleaseDate.Value.Year == request.ReleaseYear);
            }
            if (!string.IsNullOrWhiteSpace(request.DirectorSurname) && !string.IsNullOrWhiteSpace(request.DirectorSurname))
            {
                var directorQuery = _unitOfWork.DirectorRepository.GetAllQueryable();
                query = query.Join(
                    directorQuery,
                    movie=>movie.DirectorId,
                    director=>director.Id,
                    (movie, directorQuery) => new {Movie = movie,Director = directorQuery})
                    .Where(md => md.Director.name.Contains(request.DirectorName!) && md.Director.surname.Contains(request.DirectorSurname!))
                    .Select(md=>md.Movie);
            }
            return query;
        }
        private IQueryable<Movie> ApplySorting(IQueryable<Movie> query, GetMoviesByCriteriaQuery request)
        {
            if (!string.IsNullOrEmpty(request.SortByField))
            {
                var sortAbility = DictionaryOfSortAbility();
                sortAbility.TryGetValue(request.SortByField, out var sortExpression);
                if (sortExpression == null) return query;
                if (request.IsDescending)
                    return query.OrderByDescending(sortExpression);
                return query.OrderBy(sortExpression);
            }
            return query;
        }
        private static Dictionary<string, Expression<Func<Movie, object>>> DictionaryOfSortAbility()
        {
            var columns = new Dictionary<string, Expression<Func<Movie, object>>>(StringComparer.OrdinalIgnoreCase)
            {
                ["Title"] = m => m.Title,
                ["Rating"] = m => m.Stats.AverageRating!,
                ["Date"] = m => m.ReleaseDate,
                ["Director"] = m => m.DirectorId
            };
            return columns;
        }
    }
}
