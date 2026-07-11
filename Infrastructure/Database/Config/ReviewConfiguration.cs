using Domain.Entity;
using Infrastructure.Database.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Config
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r=>r.Id).ValueGeneratedNever();
            builder.HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<Media>()
                .WithMany(m => m.Reviews)
                .HasForeignKey(r => r.MediaId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.OwnsOne(r => r.AuditInfo, ai =>
            {
                ai.Property(a => a.CreatedAt).HasColumnName("CreatedAt").IsRequired();
                ai.Property(a => a.UpdatedAt).HasColumnName("UpdatedAt");
            });
            builder.OwnsOne(r => r.Rating, r =>
            {
                r.Property(r => r.Value).HasColumnName("Rating").IsRequired();
            });
            builder.Property(r => r.Username).IsRequired();
        }
    }
}
