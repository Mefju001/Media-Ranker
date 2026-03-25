using Application.Common.DTO.Response;
using MediatR;

namespace Application.Features.TvSeriesServices.GetAll
{
    public record GetAllTvSeriesQuery : IRequest<List<TvSeriesResponse>>;
}
