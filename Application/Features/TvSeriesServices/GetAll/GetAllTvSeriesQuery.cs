using MediatR;
using WebApplication1.Application.Common.DTO.Response;

namespace WebApplication1.Application.Features.TvSeries.GetAll
{
    public record GetAllTvSeriesQuery : IRequest<List<TvSeriesResponse>>;
}
