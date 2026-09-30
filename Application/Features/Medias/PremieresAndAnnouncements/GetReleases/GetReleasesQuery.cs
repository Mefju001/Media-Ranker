using Application.Features.Common.Interfaces;

namespace Application.Features.Medias.PremieresAndAnnouncements.GetReleases
{
    public record GetReleasesQuery(string scope, string mediaType) : IQuery<List<ReleaseItemResponse>>;
}
