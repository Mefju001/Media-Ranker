using MediatR;
using WebApplication1.Application.Common.DTO.Request;
using WebApplication1.Application.Common.DTO.Response;

namespace WebApplication1.Application.Features.TvSeries.AddListOfTvSeries
{
    public record AddListOfTvSeriesCommand(List<TvSeriesRequest> requests) : IRequest<List<TvSeriesResponse>>;
}
