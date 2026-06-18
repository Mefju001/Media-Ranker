using Application.Features.Common.Interfaces;
using Application.Features.Movies.Common;
using Domain.Aggregate;
using Domain.Specification;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.Movies.GetByCriteria
{
    internal class GetByCriteriaHandler : IRequestHandler<GetByCriteriaQuery, List<MovieResponse>>
    {
        private readonly IAppDbContext appDbContext;

        public GetByCriteriaHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<MovieResponse>> Handle(GetByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var moviesQuery = appDbContext.Medias.OfType<Movie>().AsNoTracking().AsQueryable();
            var genreDictionary = await appDbContext.Genres.AsNoTracking().ToDictionaryAsync(g => g.Id,g => g, cancellationToken);
            var directorDictionary = await appDbContext.Directors.AsNoTracking().ToDictionaryAsync(d => d.Id, d => d, cancellationToken);
            List<Guid>? searchGenreIds = null;
            if(request.GenreName is not null)
            {
                searchGenreIds = genreDictionary.Values
                    .Where(g => g.Name.Value.Contains(request.GenreName))
                    .Select(g => g.Id)
                    .ToList();
            }
            List<Guid>? searchDirectorIds = null;
            if(request.DirectorName is not null)
            {
                searchDirectorIds = directorDictionary.Values
                    .Where(d => d.fullname.Name.Contains(request.DirectorName)||d.fullname.Surname.Contains(request.DirectorName))
                    .Select(d => d.Id)
                    .ToList();
            }
            var specification = new MovieFilterSpecification(request.TitleSearch, request.MinRating, request.ReleaseYear, searchGenreIds, searchDirectorIds, request.SortByField, request.IsDescending);
            var results = SpecificationEvaluator.GetQuery(moviesQuery, specification);
            var response = await results.Select(m=> MovieMapper.ToMovieResponse(m, genreDictionary.GetValueOrDefault(m.GenreId), directorDictionary.GetValueOrDefault(m.DirectorId))).ToListAsync(cancellationToken);
            return response;
        }
    }
}
