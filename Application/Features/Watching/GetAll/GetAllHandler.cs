using Application.Features.Common.Interfaces;
using Application.Features.Liked.Common;
using Domain.Aggregate;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Watching.GetAll
{
    internal class GetAllHandler : IRequestHandler<GetAllQuery, List<UserInteractionsMapper>>
    {
        private readonly IAppDbContext appDbContext;
        public GetAllHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<List<UserInteractionsMapper>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.UserInteractions
                .Where(x => x.UserId == request.UserId && x.TypeInteractions == ETypeInteractions.WATCHING)
                .AsSplitQuery()
                .AsNoTracking()
                .Join(appDbContext.Set<UserDetails>(),
                    l => l.UserId, u => u.Id,
                    (watching, user) => new { watching, user })

                .Join(appDbContext.Set<Media>(),
                    t => t.watching.MediaId, m => m.Id,
                    (t, media) => new { t.watching, t.user, media })

                .Join(appDbContext.Set<Genre>(),
                    t => t.media.GenreId, g => g.Id,
                    (t, genre) => new { t.watching, t.user, t.media, genre })

                .GroupJoin(appDbContext.Set<Director>(),
                    t => (t.media as Movie).DirectorId, d => d.Id,
                    (t, directors) => new { t, directors })
                .SelectMany(
                    temp => temp.directors.DefaultIfEmpty(),
                    (temp, director) => new
                    {
                        Watching = temp.t.watching,
                        User = temp.t.user,
                        Media = temp.t.media,
                        Genre = temp.t.genre,
                        Director = director
                    })
                .Select(x => UserIntegrationsMapper.ToResponse(x.Watching, x.User, x.Media, x.Genre, x.Director))
                .ToListAsync(cancellationToken);
        }
    }
}
