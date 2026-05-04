using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Config
{
    public class ToWatchConfiguration : IEntityTypeConfiguration<ToWatch>
    {
        public void Configure(EntityTypeBuilder<ToWatch> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.UserId).HasColumnName("UserId");
            builder.Property(x => x.MediaId).HasColumnName("MediaId");
            builder.Property(x => x.LikedDate).HasColumnName("LikedDate");
        }
    }
}
