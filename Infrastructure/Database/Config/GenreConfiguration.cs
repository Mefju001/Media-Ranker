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
            builder.OwnsOne(g => g.Name, n =>
            {
                n.Property(p => p.Value)
                    .HasColumnName("Name")
                    .IsRequired()
                    .HasMaxLength(100);
                n.HasIndex(p => p.Value);
            });
        }
    }
}
