using Application.Common.Interfaces;
using Domain.Exceptions;
using MediatR;


namespace Application.Features.UserServices.ChangeDetails
{
    public class ChangeDetailsHandler : IRequestHandler<ChangeDetailsCommand, Unit>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserRepository userRepository;
        public ChangeDetailsHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
        }
        public async Task<Unit> Handle(ChangeDetailsCommand request, CancellationToken cancellationToken)
        {
            //if (request.userId < 0) throw new ArgumentOutOfRangeException("userId");
            if (request == null)
                throw new ArgumentException("you should fill that field");
            var user = await userRepository.GetUserById(request.userId) ?? throw new UserNotFoundException("Not found user");
            var emailExist = await userRepository.IsAnyUserWhoHaveEmailAndId(request.email, request.userId);
            if (emailExist) throw new EmailAlreadyExistsException("This email is taken.");
            await unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
