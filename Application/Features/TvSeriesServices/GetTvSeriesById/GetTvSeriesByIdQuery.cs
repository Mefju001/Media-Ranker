using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.TvSeriesServices.GetTvSeriesById
{
    public record GetTvSeriesByIdQuery(int id) : IQuery<TvSeriesResponse?>;

}
