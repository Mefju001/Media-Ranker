using MediatR;
using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.QueryHandler.Query
{
    public class TvSeriesQuery : IRequest<List<TvSeriesResponse>>
    {
        public string? title { get; set; }
        public string? genreName { get; set; }
        public DateTime dateTime { get; set; }
        public int? seasons { get; set; }
        public int? episodes { get; set; }
        public string? network { get; set; }
        public EStatus? status { get; set; }
        public int? isAvg { get; set; }

        public string? sortByField { get; set; }
        public bool isDescending { get; set; }
    }
}
