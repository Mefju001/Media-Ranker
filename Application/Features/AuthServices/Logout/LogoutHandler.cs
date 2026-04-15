using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.AuthServices.Logout
{
    public class LogoutHandler : IRequestHandler<LogoutCommand, Unit>
    {
        private readonly ITokenRepository tokenRepository;
        private readonly ILogger<LogoutHandler> logger;
        public LogoutHandler(ITokenRepository tokenRepository, ILogger<LogoutHandler> logger)
        {
            this.tokenRepository = tokenRepository;
            this.logger = logger;
        }
        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var deletedCount = await tokenRepository.DeleteTokensFromUserId(request.UserId, request.jti, cancellationToken);
            if (deletedCount == 0)
            {
                logger.LogWarning("Logout requested for User {UserId}, but no active tokens were found.", request.UserId);
            }
            return Unit.Value;
        }
    }
}
