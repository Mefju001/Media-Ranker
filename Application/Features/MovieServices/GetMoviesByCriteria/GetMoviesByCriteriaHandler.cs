using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Application.Features.MovieServices.GetMoviesByCriteria
{
    public class GetMoviesByCriteriaHandler : IRequestHandler<GetMoviesByCriteriaQuery, List<MovieResponse>>
    {
        private readonly IUnitOfWork context;


        public GetMoviesByCriteriaHandler(IUnitOfWork appDbContext)
        {
            context = appDbContext;
        }


        public async Task<List<MovieResponse>> Handle(GetMoviesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = context.MovieRepository.AsQueryable();
            query = await ApplyFiltersAsync(query, request);
            query = ApplySorting(query, request);
            var Response = query.Select(m => MovieMapper.ToMovieResponse(m, GetGenre(m.GenreId), GetDirector(m.DirectorId))).ToList();
            return Response;
        }
        private async Task<IQueryable<MovieDomain>> ApplyFiltersAsync(IQueryable<MovieDomain> query, GetMoviesByCriteriaQuery request)
        {
            if (!string.IsNullOrEmpty(request.TitleSearch))
            {
                query = query.Where(m => m.Title.Contains(request.TitleSearch));
            }
            if (!string.IsNullOrEmpty(request.genreName))
            {
                var genre = await context.GenreRepository.FirstOrDefaultForNameAsync(request.genreName);
                if (genre is null)
                    return query.Where(m => false);
                query = query.Where(m => m.GenreId == genre.Id);
            }
            if (request.MinRating.HasValue)
            {
                query = query.Where(m => m.Stats!.AverageRating >= request.MinRating);
            }
            if (request.ReleaseYear.HasValue)
            {
                query = query.Where(m => m.ReleaseDate.Year == request.ReleaseYear);
            }
            if (request.DirectorName is not null || request.DirectorSurname is not null)
            {
                if (string.IsNullOrEmpty(request.DirectorName)|| string.IsNullOrEmpty(request.DirectorSurname))
                {
                    throw new Exception("Both Director Name and Surname must be provided");
                }
                var director = await context.DirectorRepository.FirstOrDefaultForNameAndSurnameAsync(request.DirectorName!, request.DirectorSurname!);
                if (director is null)
                    return query.Where(m => false);
                query = query.Where(m => m.DirectorId == director.Id);
            }
            return query;
        }
        private IQueryable<MovieDomain> ApplySorting(IQueryable<MovieDomain> query, GetMoviesByCriteriaQuery request)
        {
            if (!string.IsNullOrEmpty(request.SortByField) && request.SortByField.Contains('|'))
            {
                var fields = request.SortByField.Split('|');
                if (fields.Length == 2 && bool.TryParse(fields[1], out bool IsDescending))
                {
                    request.IsDescending = IsDescending;
                    request.SortByField = fields[0];
                    Dictionary<string, Expression<Func<MovieDomain, object>>> sortAbility = DictionaryOfSortAbility();
                    sortAbility.TryGetValue(request.SortByField, out var sortExpression);
                    if (sortExpression == null) return query;
                    if (request.IsDescending)
                        return query.OrderByDescending(sortExpression);
                    return query.OrderBy(sortExpression);
                    
                }
                else throw new Exception("Mismatch");
            }
            return query;
        }
        private Dictionary<string, Expression<Func<MovieDomain, object>>> DictionaryOfSortAbility()
        {
            var columns = new Dictionary<string, Expression<Func<MovieDomain, object>>>(StringComparer.OrdinalIgnoreCase)
            {
                ["Title"] = m => m.Title,
                ["Rating"] = m => m.Stats.AverageRating!,
                ["Date"] = m => m.ReleaseDate,
                ["Director"] = m => m.DirectorId
            };
            return columns;
        }
        private GenreDomain GetGenre(int GenreId)
        {
            var genre = context.GenreRepository.Get(GenreId);
            if (genre == null) return GenreDomain.Create("Unknown");
            return genre;
        }
        private DirectorDomain GetDirector(int DirectorId)
        {
            var director = context.DirectorRepository.Get(DirectorId);
            if (director == null) return DirectorDomain.Create("Unknown", "Unknown");
            return director;
        }
    }
}
