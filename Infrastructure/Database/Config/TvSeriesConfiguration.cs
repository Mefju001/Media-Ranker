using Domain.Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Config
{
    public class TvSeriesConfiguration : IEntityTypeConfiguration<TvSeries>
    {
        public void Configure(EntityTypeBuilder<TvSeries> builder)
        {
            builder.Property(t => t.Seasons)
                .IsRequired();

            builder.Property(t => t.Episodes)
                .IsRequired();

            builder.Property(t => t.Network)
                .HasMaxLength(100);

            builder.Property(t => t.Status)
                .HasConversion<string>()
                .IsRequired();
        }
    }
}
