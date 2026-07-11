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
            builder.Property(ui => ui.Id)
                   .ValueGeneratedNever();
            builder.HasIndex(ui => new { ui.UserId, ui.MediaId })
                   .IsUnique();
            builder.HasOne<Media>()
                .WithMany()
                .HasForeignKey(r => r.MediaId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(ui => ui.TypeInteractions).IsRequired(false);
            builder.Property(ui => ui.RatingVote).IsRequired(false);

        }
    }
}
