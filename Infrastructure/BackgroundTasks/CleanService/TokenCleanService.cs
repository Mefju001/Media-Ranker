using Domain.Repository;
using Microsoft.Extensions.Logging;


namespace Infrastructure.BackgroundTasks.CleanService
{
    public class TokenCleanService : ITokenCleanService
    {
        private readonly ILogger<TokenCleanService> logger;
        private readonly ITokenRepository tokenRepository;
        public TokenCleanService(ILogger<TokenCleanService> logger, 
            ITokenRepository tokenRepository)
        {
            this.logger = logger;
            this.tokenRepository = tokenRepository;
        }
        public async Task CleanTokens(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting refresh token cleanup process.");
            try
            {
                var deletedCount = await tokenRepository.CleanUpTokensAsync(cancellationToken);
                if (deletedCount > 0)
                {
                    logger.LogInformation("Cleanup finished successfully. Deleted {Count} expired tokens.", deletedCount);
                }
                else
                {
                    logger.LogInformation("Cleanup finished. No tokens required removal.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during token cleanup");
                throw;
            }
        }
    }
}
