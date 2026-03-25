using Application.Common.DTO.Response;
using MediatR;

namespace Application.Features.TvSeriesServices.GetTvSeriesById
{
    public record GetTvSeriesByIdQuery(int id) : IRequest<TvSeriesResponse?>;

}
