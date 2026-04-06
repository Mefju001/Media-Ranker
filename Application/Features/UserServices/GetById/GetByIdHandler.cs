using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;

namespace Application.Features.UserServices.GetById
{
    public class GetByIdHandler : IRequestHandler<GetByIdQuery, UserDetailsResponse?>
    {
        private readonly IUserRepository userRepository;
        public GetByIdHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<UserDetailsResponse?> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
/*            var user = await userRepository.GetUserById(request.id, cancellationToken);
            if (user is null) return null;
            return UserMapper.ToResponse(user);*/
throw new NotImplementedException();
        }
    }
}
