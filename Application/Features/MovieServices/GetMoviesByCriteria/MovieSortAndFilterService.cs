using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Aggregate;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Features.MovieServices.GetMoviesByCriteria
{
    public class MovieSortAndFilterService : IMovieSortAndFilterService
    {
        private readonly IAppDbContext appDbContext;
        public MovieSortAndFilterService(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<List<MovieResponse>> Handler(GetMoviesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = appDbContext.Set<Movie>().AsNoTrackingWithIdentityResolution().AsSplitQuery();
            query = ApplyFilters(query,request);
            query = ApplySorting(query, request);
            return await query
                .Join(appDbContext.Set<Genre>(), mov => mov.GenreId, gen => gen.Id, (mov, gen) => new { mov, gen })
                .Join(appDbContext.Set<Director>(), mg => mg.mov.DirectorId, dir => dir.Id, (mg, dir) => new { mg.mov, mg.gen, dir })
                .Select(m=>MovieMapper.ToMovieResponse(m.mov,m.gen,m.dir))
                .ToListAsync(cancellationToken);
        }
        private IQueryable<Movie> ApplyFilters(IQueryable<Movie> query, GetMoviesByCriteriaQuery request)
        {
            if (!string.IsNullOrWhiteSpace(request.TitleSearch))
            {
                query = query.Where(m => m.Title.Contains(request.TitleSearch));
            }
            if (!string.IsNullOrWhiteSpace(request.genreName))
            {
                var genreIds = appDbContext.Set<Genre>()
                    .Where(gen => gen.Name.Value.Contains(request.genreName))
                    .Select(gen => gen.Id);

                query = query.Where(g => genreIds.Contains(g.GenreId));
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
                var genreIds = appDbContext.Set<Director>()
                    .Where(gen => gen.Name == request.DirectorName && gen.Surname == request.DirectorSurname)
                    .Select(gen => gen.Id);

                query = query.Where(g => genreIds.Contains(g.GenreId));
            }
            return query;
        }
        private IQueryable<Movie> ApplySorting(IQueryable<Movie> query, GetMoviesByCriteriaQuery request)
        {
            var sortField = request.SortByField;
            var isDescending = request.IsDescending;

            if (!string.IsNullOrEmpty(sortField) && sortField.Contains('|'))
            {
                var parts = sortField.Split('|');
                sortField = parts[0];
                isDescending = parts.Length > 1 && parts[1].ToLower() != "false";
            }

            if (!string.IsNullOrEmpty(sortField) && sortColumns.TryGetValue(sortField, out var sortExp))
            {
                return isDescending ? query.OrderByDescending(sortExp) : query.OrderBy(sortExp);
            }

            return query.OrderBy(g => g.Title);
        }
        private static readonly Dictionary<string, Expression<Func<Movie, object>>> sortColumns =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["Title"] = m => m.Title,
                ["Rating"] = m => m.Stats.AverageRating!,
                ["Date"] = m => m.ReleaseDate!.Value,
                ["Director"] = m => m.DirectorId
            };
    }
}
