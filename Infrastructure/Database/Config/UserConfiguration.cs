using Domain.Entity;
using Infrastructure.Database.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Database.Config
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
            builder.HasMany(x => x.likedMedias)
                    .WithOne()
                    .HasForeignKey(lm => lm.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
