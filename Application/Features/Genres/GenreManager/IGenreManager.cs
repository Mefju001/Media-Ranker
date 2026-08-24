using Application.Features.Genres.Common;
using Application.Features.Genres.GetGenres;

namespace Application.Features.Genres.GenreManager
{
    public interface IGenreManager
    {
        Task<IEnumerable<GenreResponse>> GetOrCreateBatchAsync(List<string> names, CancellationToken cancellationToken);
        Task<GenreResponse> GetOrCreateAsync(GenreRequest genreRequest, CancellationToken cancellationToken);
    }
}
