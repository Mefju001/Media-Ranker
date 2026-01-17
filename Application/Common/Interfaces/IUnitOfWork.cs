using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMovieRepository MovieRepository { get; }
        ILikedMediaRepository LikedMediaRepository { get; }
        IMediaRepository MediaRepository { get; }
        IUserRepository UserRepository { get; }
        ITokenRepository TokenRepository { get; }
        /*IGenericRepository<MovieDomain> Movies { get; }
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
        IGenericRepository<MediaStats> MediaStats { get; }*/


        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> CompleteAsync();
    }
}
