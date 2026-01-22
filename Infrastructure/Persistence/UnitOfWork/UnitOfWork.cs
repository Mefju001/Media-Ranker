using Application.Common.Interfaces;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
        public IGameRepository GameRepository { get; }
        public IUserRepository UserRepository { get; }
        public ITokenRepository TokenRepository { get; }
        public IMovieRepository MovieRepository { get; }
        public IDirectorRepository DirectorRepository { get; }
        public ILikedMediaRepository LikedMediaRepository => throw new NotImplementedException();
        public IMediaRepository MediaRepository => throw new NotImplementedException();
        public IGenreRepository GenreRepository { get; }
        public ITvSeriesRepository TvSeriesRepository { get; }
        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
            GameRepository = new GameRepository(context);
            UserRepository = new UserRepository(context);
            TokenRepository = new TokenRepository(context);
            MovieRepository = new MovieRepository(context);
            DirectorRepository = new DirectorRepository(context);
            GenreRepository = new GenreRepository(context);
            TvSeriesRepository = new TvSeriesRepository(context);
        }

        public async Task<int> CompleteAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return context.Database.BeginTransactionAsync();
        }
    }
}
