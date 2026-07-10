using Application.Features.Common.Interfaces;
using Application.Features.TvSeries.Common;

namespace Application.Features.TvSeries.AddRange
{
    public record AddRangeCommand(List<TvSeriesRequest> tvSeries) : ICommand<List<Guid>>, ISendNotificationCommand;
}
