using Application.Features.Common.Interfaces;
using Application.Features.Medias.Games.Common;

namespace Application.Features.Medias.Games.AddRange
{
    public record AddRangeCommand(List<GameRequest> games) : ICommand<List<Guid>>, ISendNotificationCommand;
}
