using Application.Features.Movies.Common;

namespace Application.Features.Movies.GetByCriteria
{
    public interface ISortAndFilterService
    {
        Task<List<MovieResponse>> Handler(GetByCriteriaQuery request, CancellationToken cancellationToken);
    }
}
