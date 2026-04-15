using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Domain.Aggregate;
using Domain.Entity;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.LikedServices.GetByIdLiked
{
    public class GetByIdHandler : IRequestHandler<GetByIdQuery, LikedMediaResponse?>
    {
        private readonly IAppDbContext appDbContext;
        public GetByIdHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<LikedMediaResponse?> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<LikedMedia>()
                .AsNoTracking()
                .AsSplitQuery()
                .Join(appDbContext.Set<UserDetails>(), l => l.UserId, u => u.Id, (l, u) => new { l, u })
                .Join(appDbContext.Set<Media>(), lu => lu.l.MediaId, m => m.Id, (lu, m) => new { lu.l, lu.u, m })
                .Join(appDbContext.Set<Genre>(), lum => lum.m.GenreId, g => g.Id, (lum, g) => new { lum.l, lum.u, lum.m, g })
                .Select(l =>
                new LikedMediaResponse(l.l.Id,
                 new UserDetailsResponse(l.u.Id, l.u.Fullname.Name, l.u.Fullname.Surname),
                 new MediaResponse(
                 l.m.Id,
                 l.m.Title,
                 l.m.Description,
                 new GenreResponse(l.g.Id, l.g.Name),
                 l.m.ReleaseDate,
                 l.m.Language,
                 l.m.Reviews.Select(r => new ReviewResponse(
                        r.Id,
                        r.MediaId,
                        r.Username,
                        r.Rating,
                        r.Comment,
                        r.AuditInfo.CreatedAt,
                        r.AuditInfo.UpdatedAt
                    )).ToList()),
                 l.l.LikedDate))
                .FirstOrDefaultAsync(l => l.id == request.id, cancellationToken) ?? throw new NotFoundException("Liked media not found");
        }

    }
}
