using WebApplication1.Application.Common.DTO.Request;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Services.Interfaces
{
    public interface IReferenceDataService
    {
        Task<Director> GetOrCreateDirectorAsync(DirectorRequest directorRequest);
        Task<Genre> GetOrCreateGenreAsync(GenreRequest genreRequest);
        Task<List<GenreResponse>> GetGenres();
    }
}
