using Domain.Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
