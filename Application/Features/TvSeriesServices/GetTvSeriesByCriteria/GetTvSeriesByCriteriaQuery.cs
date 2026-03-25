using Application.Common.DTO.Response;
using Domain.Enums;
using MediatR;

namespace Application.Features.TvSeriesServices.GetTvSeriesByCriteria
{
    public class GetTvSeriesByCriteriaQuery() : IRequest<List<TvSeriesResponse>>
    {
        public string? TitleSearch { get; set; }
        public double? MinRating { get; set; }
        public int? ReleaseYear { get; set; }
        public string? genreName { get; set; }
        public int? seasons { get; set; }
        public int? episodes { get; set; }
        public string? network { get; set; }
        public EStatus? status { get; set; }


        public string? SortByField { get; set; }
        public bool IsDescending { get; set; } = false;

    }

}
