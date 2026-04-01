using Domain.Aggregate;
using Domain.Base;
using Domain.Entity;
using Domain.Value_Object;
using Infrastructure.Database.Config;
using Infrastructure.Database.DBModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public class AppDbContext : IdentityDbContext<UserModel, RoleModel, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<LikedMedia> LikedMedias { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MediaConfiguration).Assembly);
        }
      
    }
}