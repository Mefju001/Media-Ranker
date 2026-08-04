using Application.Features.Common.Interfaces;
using Application.Features.Genres.GetAll;

namespace Application.Features.Genres.GetAllForMedias
{
    public record GetUsedForMediaQuery<TMedia> : IQuery<List<GenreResponse>> where TMedia:Media;
}
