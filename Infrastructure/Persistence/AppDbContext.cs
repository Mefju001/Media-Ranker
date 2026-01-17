using Application.Common.Interfaces;
using Domain.Entity;
using Infrastructure.Config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<MediaDomain> Medias { get; set; }
        public DbSet<TvSeriesDomain> TvSeries { get; set; }
        public DbSet<GameDomain> Games { get; set; }
        public DbSet<MovieDomain> Movies { get; set; }
        public DbSet<GenreDomain> Genres { get; set; }
        public DbSet<DirectorDomain> Directors { get; set; }
        public DbSet<ReviewDomain> Reviews { get; set; }
        public DbSet<UserDomain> Users { get; set; }
        public DbSet<RoleDomain> Roles { get; set; }
        public DbSet<TokenDomain> Tokens { get; set; }
        public DbSet<LikedMediaDomain> LikedMedias { get; set; }
        public DbSet<MediaStatsDomain> MediaStats { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MediaConfiguration).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}