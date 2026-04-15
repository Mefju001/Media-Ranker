using Application.Common.DTO.Request;
using Domain.Aggregate;

namespace Application.Common.Interfaces
{
    public interface IDirectorHelperService
    {
        Task<Director> GetOrCreateDirectorAsync(DirectorRequest directorRequest, CancellationToken cancellationToken);
        Task<Dictionary<(string, string), Director>> EnsureDirectorsExistAsync(List<DirectorRequest> directors, CancellationToken cancellationToken);
    }
}
