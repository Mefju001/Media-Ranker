using Application.Features.Directors.Common;


namespace Application.Features.Directors.Manager
{
    public interface IDirectorManager
    {
        Task<DirectorResponse> GetOrCreateAsync(DirectorRequest directorRequest, CancellationToken cancellationToken);
        Task<IEnumerable<DirectorResponse>> GetOrCreateBatchAsync(List<DirectorRequest> directors, CancellationToken cancellationToken);
    }
}
