using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class TokenConfiguration : IEntityTypeConfiguration<TokenDomain>
    {
        public void Configure(EntityTypeBuilder<TokenDomain> builder)
        {
            builder.HasKey(t => t.Jti);
            builder
                .HasOne<UserDomain>()
                .WithMany()
                .IsRequired()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
