using Domain.Entity;
using Infrastructure.DBModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Config
{
    public class LikedMediaConfiguration : IEntityTypeConfiguration<LikedMedia>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<LikedMedia> builder)
        {
            builder.HasIndex(lm => new { lm.userId, lm.mediaId })
                .IsUnique();
            builder
                .HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(lm => lm.userId);
            builder
                .HasOne<Media>()
                .WithMany()
                .HasForeignKey(lm => lm.mediaId);
        }
    }
}
