using Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.AuthServices.Logout
{
    public class LogoutHandler : IRequestHandler<LogoutCommand>
    {
        private readonly ITokenRepository tokenRepository;
        private readonly ILogger<LogoutHandler> logger;
        private readonly ICurrentUserContext context;
        public LogoutHandler(ITokenRepository tokenRepository, ILogger<LogoutHandler> logger, ICurrentUserContext context)
        {
            this.tokenRepository = tokenRepository;
            this.logger = logger;
            this.context = context;
        }
        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var deletedCount = await tokenRepository.DeleteTokensFromUserId(request.UserId, request.jti);
            if (deletedCount > 0)
            {
                logger.LogInformation("User {UserId} logged out (Scope: {Scope})",
                    request.UserId, request.jti ?? "All devices");
            }
        }
    }
}
