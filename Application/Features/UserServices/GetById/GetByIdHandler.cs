using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;

namespace Application.Features.UserServices.GetById
{
    public class GetByIdHandler : IRequestHandler<GetByIdQuery, UserResponse?>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserRepository userRepository;
        public GetByIdHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
        }
        public async Task<UserResponse?> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserById(request.id);
            if (user is null) return null;
            return UserMapper.ToResponse(user);
        }
    }
}
