using Domain.Exceptions;
using Domain.Repository;
using MediatR;

namespace Application.Features.Ignored.DeleteById
{
    internal class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, bool>
    {
        private readonly IUserDetailsRepository userDetailsRepository;
        public DeleteByIdHandler(IUserDetailsRepository userDetailsRepository)
        {
            this.userDetailsRepository = userDetailsRepository;
        }
        public async Task<bool> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {
            var user = await userDetailsRepository.GetByIdAsync(request.userId,cancellationToken)??throw new NotFoundException("User not found");
            user.RemoveInteraction(request.mediaId);
            return true;
        }
    }
}
