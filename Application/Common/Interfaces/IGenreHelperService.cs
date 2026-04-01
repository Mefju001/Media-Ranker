using Application.Common.DTO.Request;
using Domain.Aggregate;

namespace Application.Common.Interfaces
{
    public interface IGenreHelperService
    {
        Task<Genre> GetOrCreateGenreAsync(GenreRequest genreRequest, CancellationToken cancellationToken);
        Task<Dictionary<string, Genre>> EnsureGenresExistAsync(List<string> names, CancellationToken cancellationToken);
    }
}
