using Domain.Aggregate;
using Domain.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<T> Set<T>() where T : class;
        public DbSet<Media> Medias { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<LikedMedia> LikedMedias { get; set; }
        public DbSet<ToWatch> ToWatchlists { get; set; }
        public DbSet<UserDetails> UsersDetails { get; set; }

    }
}
