using Application.Features.Common.DTO.Response;
using Application.Features.Common.Interfaces;

namespace Application.Features.User.GetById
{
    public record GetByIdQuery(Guid id) : ICommand<UserDetailsResponse?>;
}
