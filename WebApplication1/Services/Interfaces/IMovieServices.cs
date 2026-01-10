using WebApplication1.Application.Common.DTO.Request;
using WebApplication1.Application.Common.DTO.Response;

public interface IMovieServices
{
    Task<List<MovieResponse>> GetAllAsync();
    Task<List<MovieResponse>> GetMoviesByCriteriaAsync(MovieQuery moviesQuery);
    Task<MovieResponse?> GetById(int id);
    Task<MovieResponse> Upsert(int? movieId, MovieRequest movie);
    Task<bool> Delete(int id);
}