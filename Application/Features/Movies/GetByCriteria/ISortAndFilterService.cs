using Application.Features.Movies.Common;

namespace Application.Features.Movies.GetMoviesByCriteria
{
    public interface ISortAndFilterService
    {
        Task<List<MovieResponse>> Handler(GetByCriteriaQuery request, CancellationToken cancellationToken);
    }
}
