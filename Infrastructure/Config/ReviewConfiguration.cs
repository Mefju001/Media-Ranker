using Domain.Aggregate;
using Domain.Entity;
using Infrastructure.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class ReviewConfiguration : EntityConfiguration<Review, int>
    {
        public override void Configure(EntityTypeBuilder<Review> builder)
        {
            base.Configure(builder);
            builder.HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<Media>()
                .WithMany(m => m.Reviews)
                .HasForeignKey(r => r.MediaId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.OwnsOne(r => r.AuditInfo, ai =>
            {
                ai.Property(a => a.CreatedAt).HasColumnName("CreatedAt");
                ai.Property(a => a.UpdatedAt).HasColumnName("UpdatedAt");
            });
            builder.OwnsOne(r => r.Rating, r =>
            {
                r.Property(r => r.Value).HasColumnName("Rating");
            });
            builder.OwnsOne(r => r.Username, u =>
            {
                u.Property(u => u.Value).HasColumnName("Username");
            });
        }
    }
}
