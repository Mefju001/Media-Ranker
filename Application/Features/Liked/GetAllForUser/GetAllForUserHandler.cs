using Application.Features.Common.Interfaces;
using Application.Features.Liked.Common;
using Domain.Aggregate;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Liked.GetAllForUser
{
    internal class GetAllForUserHandler : IRequestHandler<GetAllForUserQuery, List<LikedMediaResponse>>
    {
        private readonly IAppDbContext appDbContext;

        public GetAllForUserHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<LikedMediaResponse>> Handle(GetAllForUserQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<UserInteractions>()
                .AsNoTracking()
                .Where(l => l.UserId == request.userId)
                .Join(appDbContext.Set<UserDetails>(),
                    l => l.UserId, u => u.Id,
                    (like, user) => new { like, user })

                .Join(appDbContext.Set<Media>(),
                    t => t.like.MediaId, m => m.Id,
                    (t, media) => new { t.like, t.user, media })

                .Join(appDbContext.Set<Genre>(),
                    t => t.media.GenreId, g => g.Id,
                    (t, genre) => new { t.like, t.user, t.media, genre })

                .GroupJoin(appDbContext.Set<Director>(),
                    t => (t.media as Movie).DirectorId, d => d.Id,
                    (t, directors) => new { t, directors })
                .SelectMany(
                    temp => temp.directors.DefaultIfEmpty(),
                    (temp, director) => new
                    {
                        Like = temp.t.like,
                        User = temp.t.user,
                        Media = temp.t.media,
                        Genre = temp.t.genre,
                        Director = director
                    })
                .Select(x => LikedMediaMapper.ToResponse(x.Like, x.User, x.Media, x.Genre, x.Director))
                .ToListAsync(cancellationToken);
        }
    }
}
