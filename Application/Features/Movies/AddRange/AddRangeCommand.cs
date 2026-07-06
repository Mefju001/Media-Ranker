using Application.Features.Common.Interfaces;
using Application.Features.Movies.Common;

namespace Application.Features.Movies.AddRange
{
    public record AddRangeCommand(List<MovieRequest> movies) : ICommand<List<Guid>>, ISendNotificationCommand;
}
