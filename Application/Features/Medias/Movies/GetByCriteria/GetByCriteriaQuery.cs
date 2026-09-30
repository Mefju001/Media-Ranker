using Application.Features.Common.Interfaces;
using Application.Features.Medias.Movies.Common;

namespace Application.Features.Medias.Movies.GetByCriteria
{
    public record GetByCriteriaQuery(
        string? TitleSearch, double? MinRating, int? ReleaseYear,
        string? GenreName, string? DirectorName, string? DirectorSurname,
        string? SortByField, bool IsDescending) : IQuery<List<MovieResponse>>;
}
