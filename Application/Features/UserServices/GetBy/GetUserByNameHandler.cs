using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;

namespace Application.Features.UserServices.GetBy
{
    public class GetUserByNameHandler : IRequestHandler<GetUserByNameQuery, UserResponse?>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetUserByNameHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<UserResponse?> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetUserIdByUsername(request.name);
            if (user is null) return null;
            return UserMapper.ToResponse(user);
        }
    }
}
