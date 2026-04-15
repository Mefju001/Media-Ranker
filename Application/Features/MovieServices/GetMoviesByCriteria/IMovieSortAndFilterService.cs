using Application.Common.DTO.Response;

namespace Application.Features.MovieServices.GetMoviesByCriteria
{
    public interface IMovieSortAndFilterService
    {
        Task<List<MovieResponse>> Handler(GetMoviesByCriteriaQuery request, CancellationToken cancellationToken);
    }
}
