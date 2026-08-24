using Application.Features.Common.Interfaces;
using Application.Features.Genres.GetGenres;
using Application.Features.Medias.Movies.Common;
using Domain.Aggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.UserInteractions.Rankings.Get
{
    internal class GetHandler : IRequestHandler<GetQuery, List<RankingResponse>>
    {
        private readonly IAppDbContext appDbContext;
        public GetHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<List<RankingResponse>> Handle(GetQuery request, CancellationToken cancellationToken)
        {
            
            if (string.IsNullOrWhiteSpace(request.type))
            {
                throw new ArgumentException("Type cannot be null or empty.", nameof(request.type));
            }
            IQueryable<Media> query = appDbContext.Medias.AsQueryable().AsNoTracking();
            if (request.type is "movie")
            {
                query= query.OfType<Movie>();
            }
            else if(request.type is "tv")
            {
                query = query.OfType<TvSeries>();
            }
            else if(request.type is "game")
            {
                query = query.OfType<Game>();
            }
            query = query.Where(m => m.Stats.ReviewCount>=10).OrderByDescending(m => m.Stats.AverageRating).ThenByDescending(m => m.Stats.ReviewCount).Take(10);
            return await query
                .Join(appDbContext.Genres, m => m.GenreId, g => g.Id, (m, g) => new { Media = m, Genre = g })
                .Select(x => new RankingResponse(
                x.Media.Id,
                x.Media.Title,
                x.Media.Description,
                x.Media.ReleaseDate.Value,
                x.Media is Movie ? "movie" : x.Media is TvSeries ? "tv" : x.Media is Game ? "game":"unknown",
                new GenreResponse(x.Genre.Id, x.Genre.Name),
                new MediaStatsResponse(x.Media.Stats.AverageRating, x.Media.Stats.ReviewCount, x.Media.Stats.LastCalculated)
            )).ToListAsync(cancellationToken);
        }
    }
}
