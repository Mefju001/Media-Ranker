using Domain.Aggregate;
using Infrastructure.Database.DBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Config
{
    public class UserDetailsConfiguration : IEntityTypeConfiguration<UserDetails>
    {
        public void Configure(EntityTypeBuilder<UserDetails> builder)
        {
            builder.HasKey(x => x.Id);

            builder.OwnsOne(x => x.Fullname, n =>
            {
                n.Property(p => p.Name).HasColumnName("FirstName").HasMaxLength(50);
                n.Property(p => p.Surname).HasColumnName("LastName").HasMaxLength(50);
            });
            builder.OwnsOne(m => m.AuditInfo, ai =>
            {
                ai.Property(a => a.CreatedAt).HasColumnName("CreatedAt");
                ai.Property(a => a.UpdatedAt).HasColumnName("UpdatedAt");
            });
            builder.HasMany(x => x.LikedMedias)
                   .WithOne()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade); ;
            builder.Navigation(x => x.LikedMedias).UsePropertyAccessMode(PropertyAccessMode.Field);
            builder.HasOne<UserModel>()
                   .WithOne()
                   .HasForeignKey<UserDetails>(ud => ud.Id)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
