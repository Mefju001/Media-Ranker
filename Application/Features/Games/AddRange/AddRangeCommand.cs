using Application.Features.Common.Interfaces;
using Application.Features.Games.Command;

namespace Application.Features.Games.AddRange
{
    public record AddRangeCommand(List<GameRequest> games) : ICommand<List<Guid>>, ISendNotificationCommand;
}
