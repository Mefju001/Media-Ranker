using Application.Features.Common.Interfaces;
using Domain.Aggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Genres.GetGenres
{
    internal class GetGenresHandler : IRequestHandler<GetGenresQuery, List<GenreResponse>>
    {
        private readonly IAppDbContext appDbContext;
        public GetGenresHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<List<GenreResponse>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
        {
            var query = appDbContext.Genres.AsQueryable();
            if(request.Media.HasValue)
            {
                query = request.Media.Value switch
                {
                    EMediaType.Movie => query.Where(g => appDbContext.Set<Movie>().Any(m => m.GenreId == g.Id)),
                    EMediaType.TvSeries => query.Where(g => appDbContext.Set<TvSeries>().Any(s => s.GenreId == g.Id)),
                    EMediaType.Game => query.Where(g => appDbContext.Set<Game>().Any(game => game.GenreId == g.Id)),
                    _ => query
                };
            }
            return await query.Select(g => new GenreResponse(g.Id, g.Name)).ToListAsync(cancellationToken);
        }
    }
}
