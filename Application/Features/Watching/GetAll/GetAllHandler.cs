using Application.Features.Common.Interfaces;
using Application.Features.Liked.Common;
using Domain.Aggregate;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Watching.GetAll
{
    internal class GetAllHandler : IRequestHandler<GetAllQuery, List<UserInteractionsResponse>>
    {
        private readonly IAppDbContext appDbContext;
        public GetAllHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<List<UserInteractionsResponse>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var rawData = await appDbContext.UserInteractions
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId && x.TypeInteractions == ETypeInteractions.WATCHING)
                .ToListAsync(cancellationToken);
            if(!rawData.Any()) { return new List<UserInteractionsResponse>(); }
            var userIds = rawData.Select(x => x.UserId).Distinct();
            var mediaIds = rawData.Select(x => x.MediaId).Distinct();
            var users = await appDbContext.UsersDetails.Where(x => userIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, cancellationToken);
            var medias = await appDbContext.Medias.Where(x => mediaIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, cancellationToken);
            var genreIds = medias.Values.Select(x => x.GenreId).Distinct();
            var directorIds = medias.Values.OfType<Movie>().Select(x => x.DirectorId).Distinct();
            var genres = await appDbContext.Genres.Where(x => genreIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, cancellationToken);
            var directors = await appDbContext.Directors.Where(x => directorIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, cancellationToken);
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
                return UserInteractionsMapper.ToResponse(x, user, media, genre, director);
            }).ToList();
            return response;
        }
    }
}