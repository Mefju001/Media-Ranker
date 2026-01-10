using MediatR;
using WebApplication1.Application.Common.DTO.Response;

namespace WebApplication1.Application.Features.Games.GetMoviesByCriteria
{
    public class GetGamesByCriteriaQuery() : IRequest<List<GameResponse>>
    {
        public string? title { get; set; }
        public string? genreName { get; set; }
        public string? platform { get; set; }
        public string? developer { get; set; }
        public int? releaseDate { get; set; }
        public bool isAvgFiltr { get; set; }
        public int? MinRating { get; set; }


        public string? sortByField { get; set; }
        public bool IsDescending { get; set; }
    }

}
