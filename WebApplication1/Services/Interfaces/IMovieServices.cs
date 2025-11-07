using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;
using WebApplication1.QueryHandler.Query;

public interface IMovieServices
{
    Task<List<MovieResponse>> GetAllAsync();
    Task<List<MovieResponse>> GetMoviesByCriteriaAsync(MoviesQuery moviesQuery);
    Task<MovieResponse?> GetById(int id);
    Task<(int movieId, MovieResponse response)> Upsert(int? movieId, MovieRequest movie);
    Task<bool> Delete(int id);
    Task<MediaStats> testForReviews();
}