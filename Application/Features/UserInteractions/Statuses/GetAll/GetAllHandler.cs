using Application.Features.Common.Interfaces;
using Application.Features.UserInteractions.Statuses.Common;
using Domain.Aggregate;
using Domain.Entity;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.UserInteractions.Statuses.GetAll
{
    internal class GetAllHandler : IRequestHandler<GetAllQuery, List<UserInteractionsResponse>>
    {
        private readonly IAppDbContext context;
        public GetAllHandler(IAppDbContext context)
        {
            this.context = context;
        }
        public async Task<List<UserInteractionsResponse>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var userInteractions = context.UserInteractions
                .Where(ui => ui.UserId == request.UserId).AsNoTracking();

            if (request.ERatingVote.HasValue)
            {
                userInteractions = userInteractions.Where(ui => ui.RatingVote == request.ERatingVote);
            }

            if (request.ETypeInteractions.HasValue)
            {
                userInteractions = userInteractions.Where(ui => ui.TypeInteractions == request.ETypeInteractions);
            }
            var rawData = await userInteractions.ToListAsync(cancellationToken);
            if (!rawData.Any())
            {
                return new List<UserInteractionsResponse>();
            }
            var usersIds = rawData.Select(x => x.UserId).Distinct().ToList();
            var users = await context.UsersDetails.AsNoTracking().Where(u => usersIds.Contains(u.Id)).ToDictionaryAsync(u => u.Id, u => u, cancellationToken);
            var mediasIds = rawData.Select(x => x.MediaId).Distinct().ToList();
            var medias = await context.Medias.AsNoTracking().Where(m => mediasIds.Contains(m.Id)).ToDictionaryAsync(m => m.Id, m => m, cancellationToken);
            var genresIds = medias.Select(x => x.Value.GenreId).Distinct().ToList();
            var genres = await context.Genres.AsNoTracking().Where(g => genresIds.Contains(g.Id)).ToDictionaryAsync(g => g.Id, g => g, cancellationToken);
            var directorsIds = medias.Values.OfType<Movie>().Select(x => x.DirectorId).Distinct().ToList();
            var directors = await context.Directors.AsNoTracking().Where(d => directorsIds.Contains(d.Id)).ToDictionaryAsync(d => d.Id, d => d, cancellationToken);
            var response = rawData.Select(x =>
            {
                var user = users.TryGetValue(x.UserId, out var u) ? u : null;
                var media = medias.TryGetValue(x.MediaId, out var m) ? m : null;
                Genre? genre = null;
                if(media!=null)
                {
                    genres.TryGetValue(media.GenreId, out genre);
                }
                Director? director = null;
                if (media is Movie movie )
                {
                    directors.TryGetValue(movie.DirectorId, out director);
                }
                return UserInteractionsMapper.ToResponse(x, user, media, genre, director);
            }).ToList();
            return response;
        }
    }
}
