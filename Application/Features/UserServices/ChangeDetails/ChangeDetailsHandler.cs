using Application.Common.Interfaces;
using Domain.Exceptions;
using Domain.Repository;
using Domain.Value_Object;
using MediatR;


namespace Application.Features.UserServices.ChangeDetails
{
    public class ChangeDetailsHandler : IRequestHandler<ChangeDetailsCommand, Unit>
    {
        private readonly IUserDetailsRepository userDetailsRepository;
        private readonly IUserRepository userRepository;
        public ChangeDetailsHandler(IUserDetailsRepository userDetailsRepository, IUserRepository userRepository)
        {
            
            this.userDetailsRepository = userDetailsRepository;
            this.userRepository = userRepository;
        }
        public async Task<Unit> Handle(ChangeDetailsCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentException("you should fill that field");
            var user = await userDetailsRepository.GetByIdAsync(request.userId, cancellationToken) ?? throw new UserNotFoundException("Not found user");
            var emailExist = await userRepository.IsAnyUserWhoHaveEmailAndId(request.email, request.userId);
            if (emailExist) throw new EmailAlreadyExistsException("This email is taken.");
            user.UpdateProfile(new Fullname(request.name, request.surname), new Email(request.email));
            return Unit.Value;
        }
    }
}
