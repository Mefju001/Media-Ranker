using Domain.Entity;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class RoleConfiguration : IEntityTypeConfiguration<RoleDomain>
    {
        public void Configure(EntityTypeBuilder<RoleDomain> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.role).IsRequired().HasConversion<string>();
        }
    }
}
