using Domain.Aggregate;
using Domain.Entity;
using Infrastructure.Database.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Config
{
    public class LikedMediaConfiguration : IEntityTypeConfiguration<LikedMedia>
    {
        public void Configure(EntityTypeBuilder<LikedMedia> builder)
        {
            builder.HasIndex(lm => new { lm.UserId, lm.MediaId })
                .IsUnique();
            builder
                .HasOne<UserDetails>()
                .WithMany()
                .HasForeignKey(lm => lm.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasOne<Media>()
                .WithMany()
                .HasForeignKey(lm => lm.MediaId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(lm => lm.LikedDate)
                   .IsRequired();
        }
    }
}
