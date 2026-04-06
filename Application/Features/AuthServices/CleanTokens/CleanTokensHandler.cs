using MediatR;


namespace Application.Features.AuthServices.CleanTokens
{
    public class CleanTokensHandler : IRequestHandler<CleanTokensCommand, Unit>
    {
        private readonly ITokenCleanService tokenCleanupService;
        public CleanTokensHandler(ITokenCleanService tokenCleanupService)
        {
            this.tokenCleanupService = tokenCleanupService;
        }
        public async Task<Unit> Handle(CleanTokensCommand request, CancellationToken cancellationToken)
        {
            await tokenCleanupService.CleanTokens(cancellationToken);
            return Unit.Value;
        }
    }
}
