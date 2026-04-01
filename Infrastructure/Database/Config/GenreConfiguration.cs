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
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasKey(g => g.Id);

            builder.OwnsOne(g => g.Name, n =>
            {
                n.Property(p => p.Value)
                    .HasColumnName("Name")
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}
