using Domain.Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Config
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.Property(g => g.Developer)
                .IsRequired()
                .HasMaxLength(200);
            builder.HasIndex(g => g.Developer);
            builder.Property(g => g.Platforms)
                .HasColumnType("jsonb")
                .HasField("platforms")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .IsRequired();
            builder.HasIndex(g => g.Platforms).HasMethod("gin");

        }
    }
}
