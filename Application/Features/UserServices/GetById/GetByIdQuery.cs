using Application.Common.DTO.Response;
using MediatR;

namespace Application.Features.UserServices.GetById
{
    public record GetByIdQuery(Guid id) : IRequest<UserResponse?>;
}
