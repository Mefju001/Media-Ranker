using Domain.Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Config
{
    public class TvSeriesConfiguration : IEntityTypeConfiguration<TvSeries>
    {
        public void Configure(EntityTypeBuilder<TvSeries> builder)
        {
        builder.OwnsOne(x=>x.SeasonAndEpisode, sa =>
            {
                sa.Property(s => s.Seasons)
                    .HasColumnName("Season")
                    .IsRequired();
                sa.Property(s => s.Episodes)
                    .HasColumnName("Episode")
                    .IsRequired();
            });

        builder.Property(t => t.Network)
                .HasMaxLength(100);
            builder.HasIndex(t => t.Network);
            builder.Property(t => t.Status)
                .HasConversion<string>()
                .HasColumnName("TvSeriesStatus")
                .IsRequired();
            builder.HasIndex(t => t.Status);
        }
    }
}
