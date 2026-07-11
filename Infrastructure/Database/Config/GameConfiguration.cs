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
            builder.Property(g => g.Platforms)
                .HasConversion(
                    // Do bazy: Zamieniamy nasz obiekt na zwykłą tablicę string[] (PostgreSQL zapisze to jako text[])
                    v => v.Values.ToArray(),

                    // Z bazy: Bierzemy tablicę stringów z bazy i tworzymy z niej obiekt GamePlatforms
                    v => new GamePlatforms(v)
                )
                .HasColumnName("Platforms")
                .IsRequired();

            // Teraz bez problemu nakładasz indeks GIN na tę kolumnę!
            builder.HasIndex(g => g.Platforms)
                .HasMethod("gin");
        }
    }
}
