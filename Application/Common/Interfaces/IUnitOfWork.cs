using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGameRepository GameRepository { get; }
        IMovieRepository MovieRepository { get; }
        ILikedMediaRepository LikedMediaRepository { get; }
        IMediaRepository MediaRepository { get; }
        IUserRepository UserRepository { get; }
        ITokenRepository TokenRepository { get; }
        IDirectorRepository DirectorRepository { get; }
        IGenreRepository GenreRepository { get; }
        ITvSeriesRepository TvSeriesRepository { get; }
        /*
        
        IGenericRepository<Review> Reviews { get; }
        IGenericRepository<Role> Roles { get; }
        IGenericRepository<UserRole> UsersRoles { get; }
        IGenericRepository<MediaStats> MediaStats { get; }*/


        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> CompleteAsync();
    }
}
