using Domain.Aggregate;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Infrastructure.Database.Config
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {

        public void Configure(EntityTypeBuilder<Game> builder)
        {
            var platformComparer = new ValueComparer<IReadOnlyCollection<EPlatform>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()
            );
            builder.Property(g => g.Developer)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(g => g.Platforms)
                .HasColumnType("jsonb")
                .HasField("platforms")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasConversion(v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                               v => JsonSerializer.Deserialize<List<EPlatform>>(v, (JsonSerializerOptions)null) ?? new List<EPlatform>())
                .IsRequired()
                .Metadata.SetValueComparer(platformComparer);

        }
    }
}
