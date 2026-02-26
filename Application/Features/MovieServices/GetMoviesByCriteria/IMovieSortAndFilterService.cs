using Domain.Entity;

namespace Application.Features.MovieServices.GetMoviesByCriteria
{
    public interface IMovieSortAndFilterService
    {
        Task<IQueryable<Movie>> Handler(GetMoviesByCriteriaQuery request);
    }
}
