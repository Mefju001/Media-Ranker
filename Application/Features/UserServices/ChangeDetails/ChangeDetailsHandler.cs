using Domain.Exceptions;
using Domain.Repository;
using Domain.Value_Object;
using MediatR;


namespace Application.Features.UserServices.ChangeDetails
{
    public class ChangeDetailsHandler : IRequestHandler<ChangeDetailsCommand, Unit>
    {
        private readonly IUserDetailsRepository userDetailsRepository;
        public ChangeDetailsHandler(IUserDetailsRepository userDetailsRepository)
        {
            
            this.userDetailsRepository = userDetailsRepository;
        }
        public async Task<Unit> Handle(ChangeDetailsCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentException("you should fill that field");
            var user = await userDetailsRepository.GetByIdAsync(request.userId, cancellationToken) ?? throw new UserNotFoundException("Not found user");
            user.UpdateProfile(new Fullname(request.name, request.surname));
            return Unit.Value;
        }
    }
}
