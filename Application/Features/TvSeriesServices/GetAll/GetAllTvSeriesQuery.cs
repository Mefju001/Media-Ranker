using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.TvSeriesServices.GetAll
{
    public record GetAllTvSeriesQuery : IQuery<List<TvSeriesResponse>>;
}
