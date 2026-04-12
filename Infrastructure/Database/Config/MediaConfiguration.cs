using Domain.Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Config
{
    public class MediaConfiguration : IEntityTypeConfiguration<Media>
    {
        public  void Configure(EntityTypeBuilder<Media> builder)
        {
            builder.HasKey(x => x.Id);
            builder
                .HasOne<Genre>()
                .WithMany()
                .IsRequired()
                .HasForeignKey(m => m.GenreId);
            builder
                .HasDiscriminator<string>("MediaType")
                .HasValue<Movie>("Movie")
                .HasValue<TvSeries>("TvSeries")
                .HasValue<Game>("Game");
            builder.OwnsOne(m => m.Stats, sa =>
            {
                sa.Property(s => s.AverageRating).HasColumnName("AverageRating");
                sa.Property(s => s.ReviewCount).HasColumnName("reviewCount");
            });
            builder.OwnsOne(m => m.AuditInfo, ai =>
            {
                ai.Property(a => a.CreatedAt).HasColumnName("CreatedAt");
                ai.Property(a => a.UpdatedAt).HasColumnName("UpdatedAt");
            });
            builder.OwnsOne(m => m.ReleaseDate, rd =>
            {
                rd.Property(r => r.Value).HasColumnName("ReleaseYear");
            });
            builder.OwnsOne(m => m.Language, l =>
            {
                l.Property(lang => lang.Value).HasColumnName("Language");
            });
        }
    }
}
