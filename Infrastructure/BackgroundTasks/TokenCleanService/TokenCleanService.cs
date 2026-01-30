using Infrastructure.Persistence.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.BackgroundTasks.TokenCleanService
{
    public class TokenCleanService
    {
        private readonly ILogger<TokenCleanService> _logger;
        private IServiceProvider _serviceProvider;
        public TokenCleanService(IServiceProvider serviceProvider, ILogger<TokenCleanService>logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        public async Task CleanTokens()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetService(typeof(TokenRepository)) as TokenRepository;
                if (context is null) throw new ArgumentNullException(nameof(context));
                _logger.LogInformation("Starting refresh token cleanup process.");
                var tokensToDelete = await context.GetTokensToCleanUp();
                if (tokensToDelete.Any())
                {
                    _logger.LogInformation($"Found {tokensToDelete.Count} tokens to delete.");
                    await context.RemoveListOfTokens(tokensToDelete);
                }
                else
                {
                    _logger.LogError("An error occurred during token cleanup");
                }
            }
        }
    }
}
