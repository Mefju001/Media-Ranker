using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Config
{
    public class MediaStatsConfiguration : IEntityTypeConfiguration<MediaStatsDomain>
    {
        public void Configure(EntityTypeBuilder<MediaStatsDomain> builder)
        {
            builder.HasKey(ms => ms.MediaId);
            builder
                .HasOne<MediaDomain>()
                .WithOne(m => m.Stats)
                .HasForeignKey<MediaStatsDomain>(ms => ms.MediaId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
