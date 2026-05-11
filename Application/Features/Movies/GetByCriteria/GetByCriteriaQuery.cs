using Application.Features.Common.Interfaces;
using Application.Features.Movies.Common;

namespace Application.Features.Movies.GetByCriteria
{
    public record GetByCriteriaQuery(string? TitleSearch, double? MinRating, int? ReleaseYear, string? genreName, string? DirectorName, string? DirectorSurname, string? SortByField, bool IsDescending) : IQuery<List<MovieResponse>>;

}
