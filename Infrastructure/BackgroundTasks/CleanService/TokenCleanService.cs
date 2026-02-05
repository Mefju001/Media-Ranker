using Application.Common.Interfaces;
using Infrastructure.Persistence.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;


namespace Infrastructure.BackgroundTasks.CleanService
{
    public class TokenCleanService: ITokenCleanService
    {
        private readonly ILogger<TokenCleanService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        public TokenCleanService(IServiceScopeFactory serviceScopeFactory, ILogger<TokenCleanService>logger)
        {
            _scopeFactory = serviceScopeFactory;
            _logger = logger;
        }
        public async Task CleanTokens()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ITokenRepository>();
                if (context is null) throw new ArgumentNullException(nameof(context));
                _logger.LogInformation("Starting refresh token cleanup process.");
                var tokensToDelete = await context.GetTokensToCleanUp();
                if (tokensToDelete.Any())
                {
                    _logger.LogInformation($"Found {tokensToDelete.Count} tokens to delete.");
                    await context.RemoveListOfTokens(tokensToDelete);
                }
                else if (tokensToDelete.Count == 0)
                    _logger.LogInformation("There is no tokens to delete.");
                else
                {
                    _logger.LogError("An error occurred during token cleanup");
                }
            }
        }
    }
}
