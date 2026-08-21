using Application.Features.Common.Interfaces;
using MediatR;

namespace Application.Features.Medias.Movies.DeleteById
{
    public record DeleteByIdCommand(Guid id) : ICommand<Unit>, ISendNotificationCommand;
}
