using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.UserServices.GetById
{
    public record GetByIdQuery(Guid id) : ICommand<UserDetailsResponse?>;
}
