using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IReferenceDataService
    {
        Task<Director> GetOrCreateDirectorAsync(DirectorRequest directorRequest);
        Task<Genre> GetOrCreateGenreAsync(GenreRequest genreRequest);
        Task<Dictionary<string, Genre>> EnsureGenresExistAsync(List<string> names);
        Task<Dictionary<(string, string), Director>> EnsureDirectorsExistAsync(List<DirectorRequest> directors);
        Task<List<GenreResponse>> GetGenres();
        Task saveToken(Token token);
    }
}
