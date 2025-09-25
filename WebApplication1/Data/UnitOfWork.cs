using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
        public IGenericRepository<Movie> GenMovies { get; }
        public IGenericRepository<Director> GenDirectors { get; }
        public IGenericRepository<Genre> GenGenres{ get; }
        public IGenericRepository<Media> GenMedias { get; }
        public IGenericRepository<Game> GenGames { get; }
        public IGenericRepository<TvSeries>GenTvSeries { get; }
        public IGenericRepository<Review>GenReviews { get; }
        public IGenericRepository<User> GenUsers {  get; }
        public IGenericRepository<Role> GenRoles { get; }
        public IGenericRepository<Token> GenTokens { get; }
        public IGenericRepository<UserRole> GenUsersRoles { get; }
        public IGenericRepository<LikedMedia> GenLikedMedias { get; }

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

        public UnitOfWork(AppDbContext context) => this.context = context;
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
        public Task<Movie?> GetByIdAsync(int id)
            => context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        public Task<List<Movie>> GetAllAsync()
        => context.Movies.Include(m => m.genre)
                .Include(m => m.director)
                .Include(m => m.Reviews)
                    .ThenInclude(r => r.User)
                .ToListAsync();
        public void Add(Movie movie)
        {
            context.Movies.Add(movie);
        }
    }
}
