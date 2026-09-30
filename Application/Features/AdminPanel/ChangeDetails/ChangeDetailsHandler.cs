using Application.Features.Auth.Common;
using Domain.Exceptions;
using Domain.Repository;
using Domain.Value_Object;
using MediatR;

namespace Application.Features.AdminPanel.ChangeDetails
{
    internal class ChangeDetailsHandler : IRequestHandler<ChangeDetailsCommand, Unit>
    {
        private readonly IUserDetailsRepository userDetailsRepository;
        public ChangeDetailsHandler(IIdentityService identityService, IUserDetailsRepository userDetailsRepository)
        {
            this.userDetailsRepository = userDetailsRepository;
        }
        public async Task<Unit> Handle(ChangeDetailsCommand request, CancellationToken cancellationToken)
        {
            var userDetails = await userDetailsRepository.GetByIdAsync(request.userId, cancellationToken)??throw new NotFoundException("User details not found");
            var newName = string.IsNullOrWhiteSpace(request.name) ? userDetails.Fullname.FirstName : request.name;
            var newSurname = string.IsNullOrWhiteSpace(request.surname) ? userDetails.Fullname.LastName : request.surname;
            userDetails.UpdateProfile(new Fullname(request.name, request.surname));
            return Unit.Value;
        }
    }
}
