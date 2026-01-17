using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class MovieConfiguration : IEntityTypeConfiguration<MovieDomain>
    {
        public void Configure(EntityTypeBuilder<MovieDomain> builder)
        {
            builder
                .HasOne<DirectorDomain>()
                .WithMany()
                .HasForeignKey(m => m.DirectorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
