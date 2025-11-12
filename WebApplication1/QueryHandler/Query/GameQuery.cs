using MediatR;
using WebApplication1.DTO.Response;

namespace WebApplication1.QueryHandler.Query
{
    public class GameQuery:IRequest<List<GameResponse>>
    {
        public string? title { get; set; }
        public string? genreName { get; set; }

        public string? sortByField { get; set; }
        public bool IsDescending { get; set; }
    }
}
