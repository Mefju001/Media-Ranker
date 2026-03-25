using Domain.Entity;

namespace Application.Features.MovieServices.GetMoviesByCriteria
{
    public interface IMovieSortAndFilterService
    {
        IQueryable<Movie> Handler(GetMoviesByCriteriaQuery request);
    }
}
