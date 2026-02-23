using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;

namespace Application.Features.UserServices.GetBy
{
    public class GetUserByNameHandler : IRequestHandler<GetUserByNameQuery, UserResponse?>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserRepository userRepository;
        public GetUserByNameHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
        }
        public async Task<UserResponse?> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserByUsername(request.name);
            if (user is null) return null;
            return UserMapper.ToResponse(user);
        }
    }
}
