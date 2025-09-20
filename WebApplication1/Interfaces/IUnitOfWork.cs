using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
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
        Task<Movie?> GetByIdAsync(int id);
        Task<List<Movie>> GetAllAsync();
    }
}
