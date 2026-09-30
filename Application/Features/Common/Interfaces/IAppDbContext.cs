using Domain.Aggregate;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Common.Interfaces
{
    public interface IAppDbContext
    {
        public DbContext Context { get; }
        public DbSet<T> Set<T>() where T : class;
        public DbSet<Media> Medias { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Domain.Entity.UserInteractions> UserInteractions { get; set; }
        public DbSet<UserDetails> UsersDetails { get; set; }

    }
}
