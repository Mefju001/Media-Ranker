using Application.Features.Directors.Common;
using Domain.Aggregate;


namespace Application.Features.Directors_MOZE_EDYCJA.Manager
{
    public interface IDirectorManager
    {
        Task<DirectorResponse> GetOrCreateAsync(DirectorRequest directorRequest, CancellationToken cancellationToken);
        Task<IEnumerable<DirectorResponse>> GetOrCreateBatchAsync(List<DirectorRequest> directors, CancellationToken cancellationToken);
    }
}
