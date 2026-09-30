using Application.Features.Common.Interfaces;
using Application.Features.Medias.TvSeries.Common;

namespace Application.Features.Medias.TvSeries.AddRange
{
    public record AddRangeCommand(List<TvSeriesRequest> tvSeries) : ICommand<List<Guid>>, ISendNotificationCommand;
}
