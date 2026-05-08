using Application.Features.Common.Interfaces;
using Application.Features.Games.Command;
using Application.Features.Games.Common;
using Domain.Aggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Games.GetById
{
    internal class GetByIdHandler : IRequestHandler<GetByIdQuery, GameResponse?>
    {
        private readonly IAppDbContext appDbContext;
        public GetByIdHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;

        }

        public async Task<GameResponse?> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var game = await appDbContext.Set<Game>()
                .AsNoTracking()
                .Include(g => g.Stats)
                .Include(g => g.Reviews)
                .Where(m => m.Id == request.id)
                .Join(
                appDbContext.Set<Genre>(), g => g.GenreId, gen => gen.Id, (g, gen) => new { g, gen })
                .Select(g => GameMapper.ToGameResponse(g.g, g.gen))
                .FirstOrDefaultAsync(cancellationToken);
            return game;
        }
    }
}
