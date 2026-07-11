using Application.Features.Common.Interfaces;
using Application.Features.User.Common;

namespace Application.Features.User.GetById
{
    public record GetByIdQuery(Guid id) : ICommand<UserDetailsResponse?>;
}
