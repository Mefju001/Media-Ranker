using Domain.Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Config
{
    public class MediaConfiguration : EntityConfiguration<Media, int>
    {
        public override void Configure(EntityTypeBuilder<Media> builder)
        {
            base.Configure(builder);
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
            builder.HasMany(x => x.LikedMedias)
                .WithOne()
                .HasForeignKey(lm => lm.MediaId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Navigation(x => x.LikedMedias).UsePropertyAccessMode(PropertyAccessMode.Field);
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
