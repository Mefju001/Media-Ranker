using Application.Features.Genres.Common;
using Application.Features.Genres.GetAll;

namespace Application.Features.Genres.GenreManager
{
    internal interface IGenreManager
    {
        Task<IEnumerable<GenreResponse>> GetOrCreateBatchAsync(List<string> names, CancellationToken cancellationToken);
        Task<GenreResponse> GetOrCreateAsync(GenreRequest genreRequest, CancellationToken cancellationToken);
    }
}
