using Application.Features.Common.Interfaces;
using Application.Features.Medias.Movies.Common;

namespace Application.Features.Medias.Movies.AddRange
{
    public record AddRangeCommand(List<MovieRequest> movies) : ICommand<List<Guid>>, ISendNotificationCommand;
}
