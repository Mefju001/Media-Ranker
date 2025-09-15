using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
namespace WebApplication1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Media> Medias { get; set; }
        public DbSet<TvSeries>TvSeries { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Token>Tokens { get; set; }
        public DbSet<UserRole> UsersRoles { get; set; }
        public DbSet<LikedMedia>LikedMedias { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Token>()
                .HasOne(t => t.User)
                 .WithMany()
                 .HasForeignKey(t => t.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LikedMedia>()
                .HasIndex(lm => new { lm.UserId, lm.MediaId})
                .IsUnique();

            modelBuilder.Entity<LikedMedia>()
                .HasOne(lm => lm.User)
                .WithMany()
                .HasForeignKey(lm => lm.UserId);

            modelBuilder.Entity<LikedMedia>()
                .HasOne(lm => lm.Media)
                .WithMany()
                .HasForeignKey(lm => lm.MediaId);

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Media)
                .WithMany(m => m.Reviews)
                .HasForeignKey(r => r.MediaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Media>()
                .HasOne(m => m.genre)
                .WithMany()
                .HasForeignKey("genreId");
            modelBuilder.Entity<Media>()
                .HasDiscriminator<string>("MediaType")
                .HasValue<Movie>("Movie")
                .HasValue<TvSeries>("TvSeries")
                .HasValue<Game>("Game");

            modelBuilder.Entity<Role>()
                .Property(r => r.role)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }
    }
}