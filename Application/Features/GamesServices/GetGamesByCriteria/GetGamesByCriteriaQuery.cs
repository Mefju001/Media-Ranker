using Application.Common.DTO.Response;
using Application.Common.Interfaces;

namespace Application.Features.GamesServices.GetGamesByCriteria
{
    public class GetGamesByCriteriaQuery() : IQuery<List<GameResponse>>
    {
        public string? title { get; set; }
        public string? genreName { get; set; }
        public string? platform { get; set; }
        public string? developer { get; set; }
        public int? releaseDate { get; set; }
        public int? MinRating { get; set; }


        public string? sortByField { get; set; }
        public bool IsDescending { get; set; }
    }

}
