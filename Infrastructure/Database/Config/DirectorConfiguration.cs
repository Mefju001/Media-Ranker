using Domain.Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Database.Config
{
    public class DirectorConfiguration : IEntityTypeConfiguration<Director>
    {
        public void Configure(EntityTypeBuilder<Director> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(r => r.Id).ValueGeneratedNever();
            builder.OwnsOne(d => d.fullname, a =>
            {
                a.Property(p => p.Name).HasColumnName("Name").IsRequired();
                a.Property(p => p.Surname).HasColumnName("Surname").IsRequired();
                a.HasIndex(p => new { p.Name, p.Surname });
            });
        }
    }
}
