using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using Domain.Aggregate;

namespace Application.Features.MovieServices.GetMoviesByCriteria
{
    public interface IMovieSortAndFilterService
    {
        Task<List<MovieResponse>> Handler(GetMoviesByCriteriaQuery request, CancellationToken cancellationToken);
    }
}
