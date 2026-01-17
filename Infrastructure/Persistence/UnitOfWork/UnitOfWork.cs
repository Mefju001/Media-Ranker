using Application.Common.Interfaces;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
        public IUserRepository UserRepository { get; }
        public ITokenRepository TokenRepository { get; }


        public IMovieRepository MovieRepository => throw new NotImplementedException();

        public ILikedMediaRepository LikedMediaRepository => throw new NotImplementedException();

        public IMediaRepository MediaRepository => throw new NotImplementedException();

        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
            UserRepository = new UserRepository(context);
            TokenRepository = new TokenRepository(context);

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
