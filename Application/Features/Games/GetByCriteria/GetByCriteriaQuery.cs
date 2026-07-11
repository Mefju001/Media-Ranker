using Application.Features.Common.Interfaces;
using Application.Features.Games.Command;

namespace Application.Features.Games.GetByCriteria
{
    public record GetByCriteriaQuery(
        string? title, 
        string? genreName, 
        string? platform, 
        string? developer, 
        int? releaseDate,
        int? MinRating, 
        string? sortByField, 
        bool IsDescending) : IQuery<List<GameResponse>>;
}
