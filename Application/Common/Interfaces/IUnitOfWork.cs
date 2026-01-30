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
        IRoleRepository RoleRepository { get; }
        IReviewRepository ReviewRepository { get; }
        /*
        
       
        IGenericRepository<MediaStats> MediaStats { get; }*/


        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> CompleteAsync();
    }
}
