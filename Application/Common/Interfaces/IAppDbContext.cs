using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebApplication1.Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<Media> Medias { get; set; }
        public DbSet<TvSeries> TvSeries { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<UserRole> UsersRoles { get; set; }
        public DbSet<LikedMedia> LikedMedias { get; set; }
        public DbSet<MediaStats> MediaStats { get; set; }
    }
}
