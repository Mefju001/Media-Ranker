using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using Application.Common.Interfaces;

namespace Application.Features.TvSeriesServices.AddListOfTvSeries
{
    public record AddListOfTvSeriesCommand(List<TvSeriesRequest> tvSeries) : ICommand<List<TvSeriesResponse>>;
}
