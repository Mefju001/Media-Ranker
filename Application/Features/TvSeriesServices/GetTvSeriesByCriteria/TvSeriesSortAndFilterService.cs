using Application.Common.Interfaces;
using Domain.Entity;
using System.Linq.Expressions;

namespace Application.Features.TvSeriesServices.GetTvSeriesByCriteria
{
    public class TvSeriesSortAndFilterService : ITvSeriesSortAndFilterService
    {
        private readonly ITvSeriesRepository tvSeriesRepository;
        private readonly IGenreRepository genreRepository;
        public TvSeriesSortAndFilterService(ITvSeriesRepository tvSeriesRepository, IGenreRepository genreRepository)
        {
            this.tvSeriesRepository = tvSeriesRepository;
            this.genreRepository = genreRepository;
        }
        public IQueryable<TvSeries> Handler(GetTvSeriesByCriteriaQuery request)
        {
            var filteredTvSeries = ApplyFilters(request);
            var sortedTvSeries = ApplySorting(filteredTvSeries, request);
            return sortedTvSeries;
        }
        private IQueryable<TvSeries> ApplyFilters(GetTvSeriesByCriteriaQuery request)
        {
            var query = tvSeriesRepository.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.TitleSearch))
            {
                query = query.Where(m => m.Title.Contains(request.TitleSearch));
            }
            if (!string.IsNullOrWhiteSpace(request.genreName))
            {
                var genre = genreRepository.GetAllQueryable();
                query = query.Join(genre,
                    tv => tv.GenreId,
                    g => g.Id,
                    (tv, g) => new { Tv = tv, Genre = g })
                    .Where(tvg => tvg.Genre.name.Value.Contains(request.genreName))
                    .Select(tvg => tvg.Tv);
            }
            if (request.MinRating.HasValue)
            {
                query = query.Where(m => m.Stats!.AverageRating >= request.MinRating);
            }
            if (request.ReleaseYear.HasValue)
            {
                query = query.Where(m => m.ReleaseDate!.Value.Year == request.ReleaseYear);
            }
            if (!string.IsNullOrWhiteSpace(request.network))
            {
                query = query.Where(m => m.Status.ToString() == request.network);
            }
            return query;
        }
        private IQueryable<TvSeries> ApplySorting(IQueryable<TvSeries> query, GetTvSeriesByCriteriaQuery request)
        {
            if (request.SortByField != null)
            {
            var strings = request.SortByField!.Split("|");
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
        private static readonly Dictionary<string, Expression<Func<TvSeries, object>>> sortColumns =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["Title"] = m => m.Title,
                ["Rating"] = m => m.Stats.AverageRating!,
                ["Date"] = m => m.ReleaseDate!,
            };
    }
}
