using Domain.Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Config
{
    public class MediaConfiguration : IEntityTypeConfiguration<Media>
    {
        public void Configure(EntityTypeBuilder<Media> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(r => r.Id).ValueGeneratedNever();
            builder.Property(m => m.Title)
            .HasMaxLength(250)
            .IsRequired();
            builder.Property(m => m.Description)
                .HasMaxLength(2000)
                .IsRequired();
            builder.Property(m => m.Language)
                .HasMaxLength(50)
                .IsRequired();
            builder
                .HasOne<Genre>()
                .WithMany()
                .IsRequired()
                .HasForeignKey(m => m.GenreId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasDiscriminator<string>("MediaType")
                .HasValue<Movie>("Movie")
                .HasValue<TvSeries>("TvSeries")
                .HasValue<Game>("Game");
            builder.OwnsOne(m => m.Stats, sa =>
            {
                sa.ToTable("MediaStats");
                sa.WithOwner().HasForeignKey("MediaId");
                sa.HasKey("MediaId");
                sa.Property(s => s.AverageRating);
                sa.Property(s => s.ReviewCount);
                sa.HasIndex(s => s.AverageRating);
            });
            builder.OwnsOne(m => m.AuditInfo, ai =>
            {
                ai.Property(a => a.CreatedAt).HasColumnName("CreatedAt");
                ai.Property(a => a.UpdatedAt).HasColumnName("UpdatedAt");
            });
            builder.OwnsOne(m => m.ReleaseDate, rd =>
            {
                rd.Property(r => r.Value).HasColumnName("ReleaseYear");
                rd.HasIndex(r => r.Value);
            });
            builder.Navigation(m=>m.Reviews).HasField("reviews").UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
