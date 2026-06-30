using Domain.Aggregate;
using Domain.Value_Object;
using Infrastructure.Database.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Config
{
    public class UserDetailsConfiguration : IEntityTypeConfiguration<UserDetails>
    {
        public void Configure(EntityTypeBuilder<UserDetails> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.OwnsOne(m => m.Fullname, ai =>
            {
                ai.Property(a => a.FirstName).IsRequired();
                ai.Property(a => a.LastName).IsRequired();
                ai.HasIndex(p => new { p.FirstName, p.LastName });
            });
            builder.Property(x => x.Username)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(x => x.Email)
                .HasConversion(
                    v => v.ToString(),
                    v => Email.Create(v)
                )
                .HasColumnName("Email")
                .HasMaxLength(100)
                .IsRequired();
            builder.OwnsOne(m => m.AuditInfo, ai =>
            {
                ai.Property(a => a.CreatedAt).HasColumnName("CreatedAt");
                ai.Property(a => a.UpdatedAt).HasColumnName("UpdatedAt");
            });
            builder.HasOne<UserModel>()
                   .WithOne()
                   .HasForeignKey<UserDetails>(ud => ud.Id)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(u => u.UserInteractions)
               .WithOne()
               .HasForeignKey(ui => ui.UserId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
