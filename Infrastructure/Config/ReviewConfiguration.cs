using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class ReviewConfiguration : IEntityTypeConfiguration<ReviewDomain>
    {
        public void Configure(EntityTypeBuilder<ReviewDomain> builder)
        {
            builder
                .HasKey(r => r.Id);
            builder.HasOne<UserDomain>()
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.User.Id)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<MediaDomain>()
                .WithMany(m => m.Reviews)
                .HasForeignKey(r => r.Media.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
