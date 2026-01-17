using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class MediaConfiguration : IEntityTypeConfiguration<MediaDomain>
    {
        public void Configure(EntityTypeBuilder<MediaDomain> builder)
        {
            builder.HasKey(m => m.Id);
            builder.
                HasOne<GenreDomain>()
                .WithMany()
                .IsRequired()
                .HasForeignKey(g => g.GenreId);
            builder
                .HasOne<MediaStatsDomain>()
                .WithOne()
                .HasForeignKey<MediaStatsDomain>(ms => ms.MediaId);
            builder
                .HasDiscriminator<string>("MediaType")
                .HasValue<MovieDomain>("Movie")
                .HasValue<TvSeriesDomain>("TvSeries")
                .HasValue<GameDomain>("Game");
        }
    }
}
