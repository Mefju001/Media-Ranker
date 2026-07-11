using Domain.Aggregate;
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
            builder.HasOne<UserDetails>()
               .WithOne()
               .HasForeignKey<UserDetails>(ud => ud.Id);
        }
    }
}
