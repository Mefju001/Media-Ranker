using Application.Features.Common.Interfaces;
using Application.Features.Liked.Common;
using Domain.Aggregate;
using Domain.Entity;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Liked.GetAllForUser
{
    internal class GetAllForUserHandler : IRequestHandler<GetAllForUserQuery, List<UserInteractionsResponse>>
    {
        private readonly IAppDbContext appDbContext;

        public GetAllForUserHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<UserInteractionsResponse>> Handle(GetAllForUserQuery request, CancellationToken cancellationToken)
        {
            var rawData =  await appDbContext.Set<UserInteractions>()
                .AsSplitQuery()
                .AsNoTracking()
                .Where(l => l.UserId == request.userId&& l.RatingVote == ERatingVote.Liked)
                .ToListAsync(cancellationToken);
            if(!rawData.Any())
            {
                return new List<UserInteractionsResponse>();
            }
            var usersIds = rawData.Select(x => x.UserId).Distinct().ToList();
            var users = await appDbContext.UsersDetails.AsNoTracking().Where(u => usersIds.Contains(u.Id)).ToDictionaryAsync(u => u.Id, u => u, cancellationToken);
            var mediasIds = rawData.Select(x => x.MediaId).Distinct().ToList();
            var medias = await appDbContext.Medias.AsNoTracking().Where(m=>mediasIds.Contains(m.Id)).ToDictionaryAsync(m => m.Id, m => m, cancellationToken);
            var genresIds = medias.Select(x => x.Value.GenreId).Distinct().ToList();
            var genres = await appDbContext.Genres.AsNoTracking().Where(g => genresIds.Contains(g.Id)).ToDictionaryAsync(g => g.Id, g => g, cancellationToken);
            var movies = medias.Values.OfType<Movie>().ToList();
            var directorsIds = movies.Select(x => x.DirectorId).Distinct().ToList();
            var directors = await appDbContext.Directors.AsNoTracking().Where(d => directorsIds.Contains(d.Id)).ToDictionaryAsync(d => d.Id, d => d, cancellationToken);
            var response = rawData.Select(x =>
            {
                var user = users.TryGetValue(x.UserId, out var u) ? u : null;
                var media = medias.TryGetValue(x.MediaId, out var m) ? m : null;
                var genre = media != null && genres.TryGetValue(media.GenreId, out var g) ? g : null;
                Director? director = null;
                if (media is Movie movie)
                {
                    director = directors.TryGetValue(movie.DirectorId, out var d) ? d : null;
                }
                return UserIntegrationsMapper.ToResponse(x, user, media, genre, director);
            }).ToList();
            return response;
        }
    }
}
