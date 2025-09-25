using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IGenericRepository<Movie> GenMovies { get; }
        IGenericRepository<Director> GenDirectors { get; }
        IGenericRepository<Genre> GenGenres { get; }
        IGenericRepository<Media> GenMedias { get; }
        IGenericRepository<Game> GenGames { get; }
        IGenericRepository<TvSeries> GenTvSeries { get; }
        IGenericRepository<Review> GenReviews { get; }
        IGenericRepository<User> GenUsers { get; }
        IGenericRepository<Role> GenRoles { get; }
        IGenericRepository<Token> GenTokens { get; }
        IGenericRepository<UserRole> GenUsersRoles { get; }
        IGenericRepository<LikedMedia> GenLikedMedias { get; }

        DbSet<Media> Medias { get;}
        DbSet<TvSeries> TvSeries { get; }
        DbSet<Game> Games { get; }
        DbSet<Movie> Movies { get; }
        DbSet<Genre> Genres { get; }
        DbSet<Director> Directors { get; }
        DbSet<Review> Reviews { get; }
        DbSet<User> Users { get; }
        DbSet<Role> Roles { get; }
        DbSet<Token> Tokens { get; }
        DbSet<UserRole> UsersRoles { get; }
        DbSet<LikedMedia> LikedMedias { get; }
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> CompleteAsync();
    }
}
