using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IReferenceDataService
    {
        Task<DirectorDomain> GetOrCreateDirectorAsync(DirectorRequest directorRequest);
        Task<GenreDomain> GetOrCreateGenreAsync(GenreRequest genreRequest);
        Task<Dictionary<string, GenreDomain>> EnsureGenresExistAsync(List<string> names);
        Task<Dictionary<(string, string), DirectorDomain>> EnsureDirectorsExistAsync(List<DirectorRequest> directors);
        Task<List<GenreResponse>> GetGenres();
        Task saveToken(TokenDomain token);
    }
}
