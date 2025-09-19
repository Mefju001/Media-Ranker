using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;

        public DbSet<Media> Medias => context.Medias;
        public DbSet<TvSeries> TvSeries => context.TvSeries;
        public DbSet<Game> Games => context.Games;
        public DbSet<Movie> Movies => context.Movies;
        public DbSet<Genre> Genres => context.Genres;
        public DbSet<Director> Directors => context.Directors;
        public DbSet<Review> Reviews => context.Reviews;
        public DbSet<User> Users => context.Users;
        public DbSet<Role> Roles => context.Roles;
        public DbSet<Token> Tokens => context.Tokens;
        public DbSet<UserRole> UsersRoles => context.UsersRoles;
        public DbSet<LikedMedia> LikedMedias => context.LikedMedias;
        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
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
