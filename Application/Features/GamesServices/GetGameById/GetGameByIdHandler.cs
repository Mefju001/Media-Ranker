using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
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
