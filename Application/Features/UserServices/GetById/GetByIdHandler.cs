using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;

namespace Application.Features.UserServices.GetById
{
    public class GetByIdHandler : IRequestHandler<GetByIdQuery, UserResponse?>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetByIdHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<UserResponse?> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetUserById(request.id);
            if (user is null) return null;
            return UserMapper.ToResponse(user);
        }
    }
}
