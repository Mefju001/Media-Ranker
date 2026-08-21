using Application.Features.Common.Interfaces;
using Domain.Aggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Medias.Games.GetPlatforms
{
    internal class GetPlatformsHandler : IRequestHandler<GetPlatformsQuery, List<string>>
    {
        private readonly IAppDbContext appDbContext;
        public GetPlatformsHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<List<string>> Handle(GetPlatformsQuery request, CancellationToken cancellationToken)
        {
            var platforms = await appDbContext.Medias.OfType<Game>().Select(x => x.Platforms).Distinct().ToListAsync(cancellationToken);
            return platforms.SelectMany(x => x.Values).Distinct().ToList();
        }
    }
}
