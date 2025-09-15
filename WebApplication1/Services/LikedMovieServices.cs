using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO.Mapping;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class LikedMediaServices(AppDbContext context):ILikedMediaServices
    {
        private readonly AppDbContext AppDbContext = context;
        

        public async Task<(int? mediaId, LikedMediaResponse response)> Upsert(LikedMediaRequest LikedMedia)
        {
            var media = await AppDbContext.Medias.FirstOrDefaultAsync(m=>m.Id == LikedMedia.MediaId);
            var user = await AppDbContext.Users.FirstOrDefaultAsync(u=>u.Id == LikedMedia.UserId);
            if (media is null||user is null) throw new Exception("Jest pusty");
            var existingLikedMedia = await AppDbContext.LikedMedias.FirstOrDefaultAsync(lm=>lm.UserId == LikedMedia.UserId&&lm.MediaId == LikedMedia.MediaId);
            if(existingLikedMedia is not null) throw new Exception("istnieje takie polubienie");
            var likedMedia = new LikedMedia
            {
                MediaId = LikedMedia.MediaId,
                UserId = LikedMedia.UserId,
                LikedDate = DateTime.Now,
            };
            AppDbContext.LikedMedias.Add(likedMedia);
            await AppDbContext.SaveChangesAsync();
            var response = LikedMediaMapping.ToResponse(likedMedia);
            return (likedMedia.Id,response);
        }

        public async Task<bool> Delete(int userId, int mediaId)
        {
            var likedItem = await AppDbContext.LikedMedias.FirstOrDefaultAsync(lm => lm.UserId == userId&&lm.MediaId == mediaId);
            if (likedItem is not null)
            {
                //Dodać nowy wyjatek dla pustych polubionych.
                throw new Exception("Jest pusty");

            }
            AppDbContext.LikedMedias.Remove(likedItem);
            await AppDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<LikedMedia>> GetAllAsync()
        {
            var LikedItem = await AppDbContext.LikedMedias
                .ToListAsync();
            return LikedItem.ToList();
        }
        public async Task<List<Object>>GetUserLikedMedia(int userId)
        {
            /*var likedItems = await AppDbContext.LikedMedias
                .Where(lm=>lm.UserId == userId)
                .ToListAsync();
            var result = new List<Object>();

            foreach (var item in likedItems)
            {
                switch (item.Type)
                {
                    case "Movie":
                        var movie = await AppDbContext.Movies.FindAsync(item.MovieId);
                        if(movie is not null)result.Add(movie);
                        break;
                    case "TvSeries":
                        var tvSeries = await AppDbContext.TvSeries.FindAsync(item.TvSeriesId);
                        if (tvSeries is not null) result.Add(tvSeries);
                        break;
                    case "Games":
                        var game = await AppDbContext.Games.FindAsync(item.GameId);
                        if (game is not null) result.Add(game);
                        break;
                }
            }
            return result;*/
            throw new NotImplementedException();
        }
        public Task<LikedMediaResponse?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        /*public async Task<bool> Delete(int id)
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
            LikedMedia userMovieLike = new LikedMedia();
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
        }*/
    }
}
