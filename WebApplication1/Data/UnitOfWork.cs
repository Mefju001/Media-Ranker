using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
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
