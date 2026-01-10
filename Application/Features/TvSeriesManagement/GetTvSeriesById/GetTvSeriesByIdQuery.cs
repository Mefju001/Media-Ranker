using MediatR;
using WebApplication1.Application.Common.DTO.Response;

namespace WebApplication1.Application.Features.TvSeries.GetMovieById
{
    public record GetTvSeriesByIdQuery(int id) : IRequest<TvSeriesResponse?>;

}
