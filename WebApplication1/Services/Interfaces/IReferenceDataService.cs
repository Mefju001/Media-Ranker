using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.Services.Interfaces
{
    public interface IReferenceDataService
    {
        Task<Director> GetOrCreateDirectorAsync(DirectorRequest directorRequest);
        Task<Genre> GetOrCreateGenreAsync(GenreRequest genreRequest);
        Task<List<GenreResponse>> GetGenres();
    }
}
