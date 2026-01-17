using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IReferenceDataService
    {
        Task<DirectorDomain> GetOrCreateDirectorAsync(DirectorRequest directorRequest);
        Task<GenreDomain> GetOrCreateGenreAsync(GenreRequest genreRequest);
        Task<List<GenreResponse>> GetGenres();
        Task saveToken(TokenDomain token);
    }
}
