using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;

public interface IMovieServices
{
    Task<List<MovieResponse>> GetAllAsync();
    Task<List<MovieResponse>> GetSortAll(string sortByField, string sortByDirection);
    Task<List<MovieAVGResponse>> GetMoviesByAvrRating(string sortByDirection);
    Task<List<MovieResponse>> GetMovies(string? name, string? genreId, string? directorId, int? movieid);
    Task<MovieResponse?> GetById(int id);
    Task<(int movieId, MovieResponse response)> Upsert(int? movieId, MovieRequest movie);
    Task<bool> Delete(int id);
}