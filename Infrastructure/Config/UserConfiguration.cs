using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Domain.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<UserDomain>
    {
        public void Configure(EntityTypeBuilder<UserDomain> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.username).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field).HasMaxLength(50);
            builder.Property(x => x.password).IsRequired().HasMaxLength(255);
            builder.Property(x => x.name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.surname).IsRequired().HasMaxLength(100);
            builder.Property(x => x.email).IsRequired().HasMaxLength(150);
            builder.HasMany(u => u.Reviews)
               .WithOne()
               .HasForeignKey(r => r.UserId)
               .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(u => u.UserRoles)
               .WithMany()
               .UsingEntity<Dictionary<string, object>>(
               "UserRoles",
               j => j.HasOne<RoleDomain>().WithMany().HasForeignKey("RoleId"),
               j => j.HasOne<UserDomain>().WithMany().HasForeignKey("UserId")
                );
            builder.Metadata.FindNavigation(nameof(UserDomain.UserRoles))
               ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
