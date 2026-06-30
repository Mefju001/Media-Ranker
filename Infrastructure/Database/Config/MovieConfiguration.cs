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

            builder.Property(m => m.DistributionType)
                .HasConversion<string>()
                .IsRequired();
            builder.HasIndex(m => m.DistributionType);
            builder.Property(m => m.Status)
                .HasConversion<string>()
                .HasColumnName("MovieStatus")
                .IsRequired();
            builder.HasIndex(m => m.Status);
            builder.Property(m => m.DirectorId)
                .IsRequired();
            builder.HasIndex(m => m.DirectorId);
        }
    }
}
