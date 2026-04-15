using Application.Common.DTO.Response;
using Application.Common.Interfaces;

namespace Application.Features.UserServices.GetById
{
    public record GetByIdQuery(Guid id) : ICommand<UserDetailsResponse?>;
}
