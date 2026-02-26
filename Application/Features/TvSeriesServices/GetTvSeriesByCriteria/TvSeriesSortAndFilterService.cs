using Application.Common.Interfaces;
using Domain.Entity;
using System.Linq.Expressions;

namespace Application.Features.TvSeriesServices.GetTvSeriesByCriteria
{
    public class TvSeriesSortAndFilterService : ITvSeriesSortAndFilterService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITvSeriesRepository tvSeriesRepository;
        private readonly IGenreRepository genreRepository;
        public TvSeriesSortAndFilterService(IUnitOfWork unitOfWork, ITvSeriesRepository tvSeriesRepository, IGenreRepository genreRepository)
        {
            this.unitOfWork = unitOfWork;
            this.tvSeriesRepository = tvSeriesRepository;
            this.genreRepository = genreRepository;
        }
        public async Task<IQueryable<TvSeries>> Handler(GetTvSeriesByCriteriaQuery request)
        {
            var query = await tvSeriesRepository.AsQueryable();
            query = await ApplyFilters(query, request);
            query = await ApplySorting(query, request);
            return query;
        }
        private async Task<IQueryable<TvSeries>> ApplyFilters(IQueryable<TvSeries> query, GetTvSeriesByCriteriaQuery request)
        {
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
                query = query.Where(m => m.ReleaseDate.Value.Year == request.ReleaseYear);
            }
            if (!string.IsNullOrWhiteSpace(request.network))
            {
                query = query.Where(m => m.Status.ToString() == request.network);
            }
            return query;
        }
        private async Task<IQueryable<TvSeries>> ApplySorting(IQueryable<TvSeries> query, GetTvSeriesByCriteriaQuery request)
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
        private static Dictionary<string, Expression<Func<TvSeries, object>>> DictionaryOfSortAbility()
        {
            var columns = new Dictionary<string, Expression<Func<TvSeries, object>>>(StringComparer.OrdinalIgnoreCase)
            {
                ["Title"] = m => m.Title,
                ["Rating"] = m => m.Stats.AverageRating!,
                ["Date"] = m => m.ReleaseDate,
            };
            return columns;
        }
    }
}
