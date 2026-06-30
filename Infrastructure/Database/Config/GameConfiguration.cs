using Domain.Aggregate;
using Domain.Value_Object;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Config
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.OwnsOne(g => g.Details, d =>
            {
                d.Property(p => p.Developer)
                    .HasColumnName("Developer")
                    .IsRequired()
                    .HasMaxLength(200);
                d.HasIndex(p => p.Developer);
                d.Property(p => p.Engine)
                    .HasColumnName("Engine")
                    .IsRequired()
                    .HasMaxLength(200);
                d.HasIndex(p => p.Engine);

            });
            builder.Property(p => p.Status)
                    .HasColumnName("GameStatus")
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .IsRequired();
            builder.Property(g => g.PegiRating)
            .HasConversion(
                pegi => pegi.AgeValue,                     
                value => PegiRating.FromValue(value))       
            .HasColumnName("PegiRating")
            .IsRequired();
            builder.Property(g=>g.SupportsCrossPlay)
                .HasColumnName("SupportsCrossPlay")
                .IsRequired();
            builder.OwnsOne(g => g.Platforms, p =>
            {
                p.ToJson("Platforms");
                p.Property(x=>x.Values).IsRequired();
            });
            builder.HasIndex("Platforms").HasMethod("gin");

        }
    }
}
