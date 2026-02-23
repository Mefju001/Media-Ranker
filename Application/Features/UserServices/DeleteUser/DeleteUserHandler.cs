using Application.Common.Interfaces;
using MediatR;


namespace Application.Features.UserServices.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserRepository userRepository;

        public DeleteUserHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            //if (request.id < 0) throw new ArgumentOutOfRangeException("id");
            await userRepository.DeleteUser(request.id);
            await unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
