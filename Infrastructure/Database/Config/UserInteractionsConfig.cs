using Domain.Aggregate;
using Domain.Entity;
using Infrastructure.Database.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Database.Config
{
    public class UserInteractionsConfig : IEntityTypeConfiguration<UserInteractions>
    {
        public void Configure(EntityTypeBuilder<UserInteractions> builder)
        {

            builder.HasKey(ui=>ui.Id);
            builder.HasIndex(ui => new { ui.UserId, ui.MediaId })
                   .IsUnique();
            builder.HasOne<UserModel>()
                   .WithMany()
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<UserDetails>()
                .WithMany(m => m.UserInteractions)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(ui => ui.TypeInteractions).IsRequired(false);
            builder.Property(ui => ui.RatingVote).IsRequired(false);
        }
    }
}
