using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Config
{
    public class LikedMediaConfiguration : IEntityTypeConfiguration<LikedMediaDomain>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<LikedMediaDomain> builder)
        {
            builder.HasIndex(lm => new { lm.userId, lm.mediaId })
                .IsUnique();
            builder
                .HasOne<UserDomain>()
                .WithMany()
                .HasForeignKey(lm => lm.userId);
            builder
                .HasOne<MediaDomain>()
                .WithMany()
                .HasForeignKey(lm => lm.mediaId);
        }
    }
}
