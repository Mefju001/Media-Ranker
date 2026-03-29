using Domain.Entity;
using Infrastructure.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.ToTable("AspNetUsers");

            builder.HasMany<Review>()
                   .WithOne()
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(x => x.Roles)
                   .WithOne()
                   .HasForeignKey(ur => ur.UserId)
                   .IsRequired();


            builder.HasMany(u => u.Tokens)
                   .WithOne()
                   .HasForeignKey(t => t.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
