using Domain.Entity;
using Infrastructure.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder
                .HasKey(r => r.Id);
            builder.HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<Media>()
                .WithMany(m => m.Reviews)
                .HasForeignKey(r => r.MediaId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.OwnsOne(r => r.Rating, d =>
            {
                d.Property(x => x.value).HasColumnName("Rating");
            });
            builder.OwnsOne(r => r.Username, d =>
            {
                d.Property(x => x.Value).HasColumnName("Username");
            });
        }
    }
}
