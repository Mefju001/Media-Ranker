using Application.Features.Common.Interfaces;
using MediatR;

namespace Application.Features.Medias.TvSeries.DeleteById
{
    public record DeleteByIdCommand(Guid id) : ICommand<Unit>, ISendNotificationCommand;
}
