using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Aggregate;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.LikedServices.GetAllLiked
{
    public class GetAllHandler : IRequestHandler<GetAllQuery, List<LikedMediaResponse>>
    {
        private readonly IAppDbContext appDbContext;
        public GetAllHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<List<LikedMediaResponse>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {

            return await appDbContext.Set<LikedMedia>()
                .AsNoTrackingWithIdentityResolution()
                .AsSplitQuery()
                .Join(appDbContext.Set<UserDetails>(), l => l.UserId, u => u.Id, (l, u) => new { l, u })
                .Join(appDbContext.Set<Media>(), lu => lu.l.MediaId, m => m.Id, (lu, m) => new { lu.l, lu.u, m })
                .Join(appDbContext.Set<Genre>(), lum => lum.m.GenreId, g => g.Id, (lum, g) => new { lum.l, lum.u, lum.m, g })
                .GroupJoin(appDbContext.Set<Director>(), x => (x.m as Movie).DirectorId, d => d.Id, (x, directors) => new { x, directors })
                .SelectMany(temp => temp.directors.DefaultIfEmpty(), (temp, director) => new { temp.x, director })
                .Select(l => LikedMediaMapper.ToResponse(l.x.l, l.x.u, l.x.m, l.x.g, l.director))
                .ToListAsync(cancellationToken);
        }
    }
}
