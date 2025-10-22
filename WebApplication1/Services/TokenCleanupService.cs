using WebApplication1.Data;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class TokenCleanupService : ITokenCleanupService
    {
        public readonly ILogger<TokenCleanupService> logger;
        public readonly IServiceProvider ServiceProvider;

        public TokenCleanupService(IServiceProvider serviceProvider, ILogger<TokenCleanupService> logger)
        {
            this.logger = logger;
            ServiceProvider = serviceProvider;
        }

        public async Task Cleanup()
        {
            logger.LogInformation("Zaczynamy czyszczenie bazy danych z tokenów");
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<AppDbContext>();

                try
                {
                    var tokensToDelete = context.Tokens.Where(t => (t.IsRevoked == true) || (t.ExpiryDate < DateTime.UtcNow)).ToList();
                    if (tokensToDelete.Any())
                    {
                        logger.LogInformation($"znaleziono {tokensToDelete.Count}");
                        context.Tokens.RemoveRange(tokensToDelete);
                        await context.SaveChangesAsync();
                    }
                    else
                        logger.LogInformation("Nic nie znaleziono");

                }
                catch (Exception ex)
                {
                    logger.LogInformation("Wystapił błąd " + ex.Message);
                }
            }
        }
    }
}
