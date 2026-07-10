using Application.Features.Common.Interfaces;
using MediatR;

namespace Application.Features.Movies.DeleteById
{
    public record DeleteByIdCommand(Guid id) : ICommand<Unit>, ISendNotificationCommand;
}
