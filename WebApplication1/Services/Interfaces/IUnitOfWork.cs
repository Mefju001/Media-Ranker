using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services.Interfaces
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


        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> CompleteAsync();
    }
}
