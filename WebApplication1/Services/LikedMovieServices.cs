using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO.Mapping;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Migrations;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class LikedMovieServices(AppDbContext context):ILikedMovieServices
    {
        private readonly AppDbContext AppDbContext = context;

        public async Task<bool> Delete(int id)
        {
            var likedMovie = await AppDbContext.UserMovieLike.FirstOrDefaultAsync(x => x.Id == id);
            if (likedMovie != null)
            {
                AppDbContext.UserMovieLike.Remove(likedMovie);
                await AppDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<LikedMovieResponse>> GetAllAsync()
        {
            var likedMovies = await AppDbContext.UserMovieLike
                .Include(uml => uml.movie)
                    .ThenInclude(m => m.genre)
                .Include(uml => uml.movie)
                    .ThenInclude(m=>m.director)
                .Include(uml => uml.movie)
                    .ThenInclude(m => m.Reviews)
                    .ThenInclude(r=>r.User)
                .Include(uml => uml.user)
                    .ThenInclude(u=>u.UserRoles)
                        .ThenInclude(ur=>ur.Role)
                .ToListAsync();
            return likedMovies.Select(LikedMovieMapping.ToResponse).ToList();
        }

        public async Task<LikedMovieResponse?> GetById(int id)
        {
            var likedMovies = await AppDbContext.UserMovieLike
                    .Include(uml => uml.movie)
                        .ThenInclude(m => m.genre)
                    .Include(uml => uml.movie)
                        .ThenInclude(m => m.director)
                    .Include(uml => uml.movie)
                        .ThenInclude(m => m.Reviews)
                        .ThenInclude(r => r.User)
                    .Include(uml => uml.user)
                        .ThenInclude(u => u.UserRoles)
                            .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(uml=>uml.Id == id);
            if (likedMovies == null) throw new Exception("Not found liked movie like this");
            return LikedMovieMapping.ToResponse(likedMovies);
        }

        public async Task<(int movieId, LikedMovieResponse response)> Add(LikedMovieRequest likedMovie)
        {
            var existingLiked = await AppDbContext.UserMovieLike.FirstOrDefaultAsync(uml => uml.movieId == likedMovie.movieId && uml.userId == likedMovie.userId);
            if(existingLiked is not null) throw new Exception("You liked this");
            var user = await AppDbContext.Users.FirstOrDefaultAsync(u=>u.Id == likedMovie.userId);
            var movie = await AppDbContext.Movies.FirstOrDefaultAsync(u => u.Id == likedMovie.movieId);
            if(user is null|| movie is null)
            {
                throw new Exception("Not find user or movie");
            }
            UserMovieLike userMovieLike = new UserMovieLike();
            userMovieLike.movie = movie;
            userMovieLike.user = user;
            AppDbContext.UserMovieLike.Add(userMovieLike);
            await AppDbContext.SaveChangesAsync();
            var response = await AppDbContext.UserMovieLike
                    .Include(uml => uml.movie)
                        .ThenInclude(m => m.genre)
                    .Include(uml => uml.movie)
                        .ThenInclude(m => m.director)
                    .Include(uml => uml.movie)
                        .ThenInclude(m => m.Reviews)
                        .ThenInclude(r => r.User)
                    .Include(uml => uml.user)
                        .ThenInclude(u => u.UserRoles)
                            .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(uml => uml.Id == userMovieLike.Id);
            return (movie.Id, LikedMovieMapping.ToResponse(response));
        }
    }
}
