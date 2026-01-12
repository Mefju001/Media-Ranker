using Application.Common.Interfaces;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics.Contracts;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Domain.Entities;
using WebApplication1.Infrastructure.Persistence.Repository;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
        public IUserRepository UserRepository { get; }
        public ITokenRepository TokenRepository {  get; }
        public IGenericRepository<MediaStats> MediaStats { get; }
        public IGenericRepository<Movie> Movies { get; }
        public IGenericRepository<Director> Directors { get; }
        public IGenericRepository<Genre> Genres { get; }
        public IGenericRepository<Media> Medias { get; }
        public IGenericRepository<Game> Games { get; }
        public IGenericRepository<TvSeries> TvSeries { get; }
        public IGenericRepository<Review> Reviews { get; }
        public IGenericRepository<User> Users { get; }
        public IGenericRepository<Role> Roles { get; }
        public IGenericRepository<Token> Tokens { get; }
        public IGenericRepository<UserRole> UsersRoles { get; }
        public IGenericRepository<LikedMedia> LikedMedias { get; }

        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
            UserRepository = new UserRepository(context);
            TokenRepository = new TokenRepository(context);
            MediaStats = new GenericRepository<MediaStats>(context);
            Movies = new GenericRepository<Movie>(context);
            Directors = new GenericRepository<Director>(context);
            Genres = new GenericRepository<Genre>(context);
            Medias = new GenericRepository<Media>(context);
            Games = new GenericRepository<Game>(context);
            TvSeries = new GenericRepository<TvSeries>(context);
            Reviews = new GenericRepository<Review>(context);
            Users = new GenericRepository<User>(context);
            Roles = new GenericRepository<Role>(context);
            Tokens = new GenericRepository<Token>(context);
            UsersRoles = new GenericRepository<UserRole>(context);
            LikedMedias = new GenericRepository<LikedMedia>(context);
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
