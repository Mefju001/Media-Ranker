using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Domain.Aggregate;
using Domain.Entity;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.LikedServices.GetAllLikedByUser
{
    public class GetAllLikedByUserHandler : IRequestHandler<GetAllLikedByUserQuery, List<LikedMediaResponse>>
    {
        private readonly IAppDbContext appDbContext;

        public GetAllLikedByUserHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<LikedMediaResponse>> Handle(GetAllLikedByUserQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<LikedMedia>()
                .Where(l => l.UserId == request.userId)
                .AsNoTrackingWithIdentityResolution()
                .AsSplitQuery()
                .Join(appDbContext.Set<UserDetails>(), l => l.UserId, u => u.Id, (l, u) => new { l, u })
                .Join(appDbContext.Set<Media>(), lu => lu.l.MediaId, m => m.Id, (lu, m) => new { lu.l, lu.u, m })
                .Join(appDbContext.Set<Genre>(), lum => lum.m.GenreId, g => g.Id, (lum, g) => new { lum.l, lum.u, lum.m, g })
                .Select(l => new LikedMediaResponse
                (l.l.Id,
                 new UserDetailsResponse(l.u.Id, l.u.Fullname.Name, l.u.Fullname.Surname, l.u.email),
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
                .ToListAsync(cancellationToken)
                ?? throw new NotFoundException($"No liked media found for user with ID {request.userId}");
        }
    }
}
