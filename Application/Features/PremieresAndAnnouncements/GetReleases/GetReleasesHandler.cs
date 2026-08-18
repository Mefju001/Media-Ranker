using Application.Features.Common.Interfaces;
using Application.Features.Genres.GetAll;
using Application.Features.Movies.Common;
using Domain.Aggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PremieresAndAnnouncements.GetReleases
{
    internal class GetReleasesHandler : IRequestHandler<GetReleasesQuery, List<ReleaseItemResponse>>
    {
        private readonly IAppDbContext appDbContext;
        public GetReleasesHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<List<ReleaseItemResponse>> Handle(GetReleasesQuery request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrWhiteSpace(request.scope) || string.IsNullOrWhiteSpace(request.mediaType))
            {
                throw new ArgumentException("Scope and mediaType must be provided.");
            }
            IQueryable<Media> releaseItemResponses = appDbContext.Set<Media>().AsQueryable().AsNoTracking();
            if(request.mediaType is "movie")
            {
                releaseItemResponses = releaseItemResponses.OfType<Movie>();
            }
            else if(request.mediaType is "tv")
            {
                releaseItemResponses = releaseItemResponses.OfType<Domain.Aggregate.TvSeries>();
            }
            else if(request.mediaType is "game")
            {
                releaseItemResponses = releaseItemResponses.OfType<Game>();
            }
            if (request.scope is "upcoming")
            {
                releaseItemResponses = releaseItemResponses.Where(m => m.ReleaseDate!.Value >= DateTime.UtcNow).OrderBy(m => m.ReleaseDate.Value);
            }
            if(request.scope is "recent")
            {
                releaseItemResponses = releaseItemResponses.Where(m => m.ReleaseDate!.Value >= DateTime.UtcNow.AddDays(-30) && m.ReleaseDate.Value <= DateTime.UtcNow).OrderByDescending(m => m.ReleaseDate.Value);
            }
            var results = await releaseItemResponses
                .Select(d =>new ReleaseItemResponse
                (
                    d.Id,
                    d.Title,
                    d.Description,
                    d.ReleaseDate!.Value,
                    d is Movie ? "movie" : d is Domain.Aggregate.TvSeries ? "tvSeries" : d is Game ? "game" : "unknown",
                    appDbContext.Genres.Where(g => g.Id == d.GenreId).Select(g => new GenreResponse(g.Id, g.Name)).FirstOrDefault(),
                    new MediaStatsResponse(d.Stats.AverageRating, d.Stats.ReviewCount, d.Stats.LastCalculated)
                )).ToListAsync(cancellationToken);
            return results;
        }
    }
}
