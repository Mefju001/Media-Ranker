using Domain.Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Database.Config
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasKey(g => g.Id);
            builder.Property(r => r.Id).ValueGeneratedNever();
            builder.Property(g => g.Name).IsRequired().HasMaxLength(100);
            builder.HasIndex(g => g.Name).IsUnique();
        }
    }
}
