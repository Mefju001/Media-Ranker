using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Domain.Aggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.GamesServices.GetGameById
{
    public class GetGameByIdHandler : IRequestHandler<GetGameByIdQuery, GameResponse?>
    {
        private readonly IAppDbContext appDbContext;
        public GetGameByIdHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;

        }

        public async Task<GameResponse?> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
        {
            var game = await appDbContext.Set<Game>()
                .AsNoTracking()
                .AsSplitQuery()
                .Include(g => g.Stats)
                .Include(g => g.Reviews)
                .Where(m => m.Id == request.id)
                .Join(
                appDbContext.Set<Genre>(), g => g.GenreId, gen => gen.Id, (g, gen) => new { g, gen })
                .Select(g => new GameResponse(
                    g.g.Id,
                    g.g.Title,
                    g.g.Description,
                    new GenreResponse(g.g.GenreId, g.gen.Name),
                    g.g.ReleaseDate!,
                    g.g.Language,
                    g.g.Reviews.Select(r => new ReviewResponse(
                        r.Id,
                        r.MediaId,
                        r.Username,
                        r.Rating,
                        r.Comment,
                        r.AuditInfo.CreatedAt,
                        r.AuditInfo.UpdatedAt
                    )).ToList(),
                    new MediaStatsResponse(
                        g.g.Stats.AverageRating,
                        g.g.Stats.ReviewCount,
                        g.g.Stats.LastCalculated
                    ),
                    g.g.Developer,
                    g.g.Platform
                ))
                .FirstOrDefaultAsync(cancellationToken);
            return game;
        }
    }
}
