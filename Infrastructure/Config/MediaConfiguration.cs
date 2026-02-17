using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class MediaConfiguration : IEntityTypeConfiguration<Media>
    {
        public void Configure(EntityTypeBuilder<Media> builder)
        {
            builder.HasKey(m => m.Id);
            builder
                .HasOne<Genre>()
                .WithMany()
                .IsRequired()
                .HasForeignKey(m => m.GenreId);
            builder.OwnsOne(m => m.Language, d =>
            {
                d.Property(x => x.value).HasColumnName("Language");
            });
            builder.OwnsOne(m => m.ReleaseDate, d =>
            {
                d.Property(x => x.Value).HasColumnName("ReleaseDate");
            });
            builder.OwnsOne(m => m.Stats, s => {
                s.Property<int>("MediaId");
                s.HasKey("MediaId");
                s.WithOwner().HasForeignKey("MediaId");
                s.ToTable("MediaStats");
            });
            builder
                .HasDiscriminator<string>("MediaType")
                .HasValue<Movie>("Movie")
                .HasValue<TvSeries>("TvSeries")
                .HasValue<Game>("Game");

        }
    }
}
