using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using MediatR;

namespace Application.Features.TvSeriesServices.AddListOfTvSeries
{
    public record AddListOfTvSeriesCommand(List<TvSeriesRequest> requests) : IRequest<List<TvSeriesResponse>>;
}
