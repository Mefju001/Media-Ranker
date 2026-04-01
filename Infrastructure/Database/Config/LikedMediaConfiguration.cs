using Domain.Entity;
using Infrastructure.Database.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Config
{
    public class LikedMediaConfiguration : EntityConfiguration<LikedMedia,int>
    {
        public override void Configure(EntityTypeBuilder<LikedMedia> builder)
        {
            builder.HasIndex(lm => new { lm.UserId, lm.MediaId })
                .IsUnique();
            builder
                .HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(lm => lm.UserId)
                .OnDelete(DeleteBehavior.Cascade); ;
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
