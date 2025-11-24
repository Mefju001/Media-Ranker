using Microsoft.EntityFrameworkCore.Storage;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Movie> Movies { get; }
        IGenericRepository<Director> Directors { get; }
        IGenericRepository<Genre> Genres { get; }
        IGenericRepository<Media> Medias { get; }
        IGenericRepository<Game> Games { get; }
        IGenericRepository<TvSeries> TvSeries { get; }
        IGenericRepository<Review> Reviews { get; }
        IGenericRepository<User> Users { get; }
        IGenericRepository<Role> Roles { get; }
        IGenericRepository<Token> Tokens { get; }
        IGenericRepository<UserRole> UsersRoles { get; }
        IGenericRepository<LikedMedia> LikedMedias { get; }
        IGenericRepository<MediaStats> MediaStats { get; }


        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> CompleteAsync();
    }
}
