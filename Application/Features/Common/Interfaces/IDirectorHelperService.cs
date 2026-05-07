using Application.Features.Common.DTO.Request;
using Domain.Aggregate;

namespace Application.Features.Common.Interfaces
{
    public interface IDirectorHelperService
    {
        Task<Director> GetOrCreateDirectorAsync(DirectorRequest directorRequest, CancellationToken cancellationToken);
        Task<Dictionary<(string, string), Director>> EnsureDirectorsExistAsync(List<DirectorRequest> directors, CancellationToken cancellationToken);
    }
}
