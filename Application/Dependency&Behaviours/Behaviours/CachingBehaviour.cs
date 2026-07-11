using Application.Features.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Dependency_Behaviours.Behaviours
{
    public class CachingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : IQuery<TResponse>
    {
        private readonly IMemoryCache memoryCache;
        public CachingBehaviour(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is not ICachedQuery cachedQuery)
            {
                return await next();
            }

            if (memoryCache.TryGetValue(cachedQuery.CacheKey, out TResponse? cachedResponse))
            {
                return cachedResponse!;
            }

            var freshResponse = await next();

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cachedQuery.Expiration ?? TimeSpan.FromMinutes(5)
            };

            memoryCache.Set(cachedQuery.CacheKey, freshResponse, cacheOptions);

            return freshResponse;
        }
    }
}
