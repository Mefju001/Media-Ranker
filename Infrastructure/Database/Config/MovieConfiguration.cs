using Domain.Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Config
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.OwnsOne(m => m.Duration, d =>
            {
                d.Property(p => p.Value)
                    .HasColumnName("DurationMinutes")
                    .IsRequired();
            });

            builder.Property(m => m.IsCinemaRelease)
                .HasColumnName("IsCinemaRelease");

            builder.Property(m => m.DirectorId)
                .IsRequired();
        }
    }
}
