using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IReferenceDataService
    {
        Task<Director> GetOrCreateDirectorAsync(DirectorRequest directorRequest, CancellationToken cancellationToken);
        Task<Genre> GetOrCreateGenreAsync(GenreRequest genreRequest, CancellationToken cancellationToken);
        Task<Dictionary<string, Genre>> EnsureGenresExistAsync(List<string> names, CancellationToken cancellationToken);
        Task<Dictionary<(string, string), Director>> EnsureDirectorsExistAsync(List<DirectorRequest> directors, CancellationToken cancellationToken);
        Task<List<GenreResponse>> GetGenres(CancellationToken cancellationToken);
        Task saveToken(Token token, CancellationToken cancellationToken);
    }
}
