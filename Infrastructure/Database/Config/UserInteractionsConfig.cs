using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Database.Config
{
    public class UserInteractionsConfig : IEntityTypeConfiguration<UserInteractions>
    {
        public void Configure(EntityTypeBuilder<UserInteractions> builder)
        {
            builder.HasKey(ui => new { ui.UserId, ui.MediaId });


            builder.Property(ui => ui.TypeInteractions).IsRequired(false);
            builder.Property(ui => ui.RatingVote).IsRequired(false);
        }
    }
}
