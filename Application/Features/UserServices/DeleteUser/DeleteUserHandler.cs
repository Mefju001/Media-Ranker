using Application.Common.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Features.UserServices.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<DeleteUserHandler> logger;

        public DeleteUserHandler(IUserRepository userRepository, ILogger<DeleteUserHandler> logger)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserById(request.id, cancellationToken);
            if (user is null)
            {
                logger.LogWarning("User with id {id} not found.", request.id);
                throw new NotFoundException($"User with id {request.id} not found.");
            }
            var result = await userRepository.DeleteUser(user);
            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault()?.Description ?? "Unknown error";
                logger.LogError("Failed to delete user {UserId}: {Error}", request.id, error);
                throw new Exception($"Could not delete user: {error}");
            }
            logger.LogInformation("User with id {id} successfully deleted.", request.id);
            return Unit.Value;
        }
    }
}
