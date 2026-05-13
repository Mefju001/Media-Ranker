using Application.Features.Common.Interfaces;
using Application.Features.Liked.Common;
using Domain.Aggregate;
using Domain.Entity;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Liked.GetById
{
    internal class GetByIdHandler : IRequestHandler<GetByIdQuery, LikedResponse?>
    {
        private readonly IAppDbContext appDbContext;
        public GetByIdHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<LikedResponse?> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<UserInteractions>()
                .AsNoTracking()
                .AsSplitQuery()
                .Join(appDbContext.Set<UserDetails>(), l => l.UserId, u => u.Id, (l, u) => new { l, u })
                .Join(appDbContext.Set<Media>(), lu => lu.l.MediaId, m => m.Id, (lu, m) => new { lu.l, lu.u, m })
                .Join(appDbContext.Set<Genre>(), lum => lum.m.GenreId, g => g.Id, (lum, g) => new { lum.l, lum.u, lum.m, g })
                .Select(x => LikedMapper.ToResponse(x.l, x.u, x.m, x.g, null))
                .FirstOrDefaultAsync(l => l.id == request.id, cancellationToken) ?? throw new NotFoundException("Liked media not found");
        }

    }
}
