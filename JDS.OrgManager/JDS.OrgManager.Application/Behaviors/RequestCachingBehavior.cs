// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.Queries;
using JDS.OrgManager.Application.Abstractions.Serialization;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Behaviors
{
    public class RequestCachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private static readonly DistributedCacheEntryOptions defaultCacheOptions = new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromHours(1.0) };

        private readonly IByteSerializer byteSerializer;

        private readonly IDistributedCache cache;

        private readonly ILogger logger;

        public RequestCachingBehavior(IDistributedCache cache, IByteSerializer byteSerializer, ILogger<TResponse> logger)
        {
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
            this.byteSerializer = byteSerializer ?? throw new ArgumentNullException(nameof(byteSerializer));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is ICacheableQuery cacheableQuery)
            {
                TResponse response;
                async Task<TResponse> GetResponseAndAddToCache()
                {
                    response = await next();
                    var options = cacheableQuery.SlidingExpiration != null ? new DistributedCacheEntryOptions { SlidingExpiration = cacheableQuery.SlidingExpiration } : defaultCacheOptions;
                    await cache.SetAsync(cacheableQuery.CacheKey, byteSerializer.Serialize(response), options, cancellationToken);
                    return response;
                }

                if (cacheableQuery.ReplaceCachedEntry)
                {
                    logger.LogInformation($"Replacing cache entry for key '{cacheableQuery.CacheKey}'.");
                    response = await GetResponseAndAddToCache();
                }
                else
                {
                    var cachedResponse = await cache.GetAsync(cacheableQuery.CacheKey, cancellationToken);
                    if (cachedResponse != null)
                    {
                        logger.LogInformation($"Cache hit for key '{cacheableQuery.CacheKey}'.");
                        response = byteSerializer.Deserialize<TResponse>(cachedResponse);
                    }
                    else
                    {
                        logger.LogInformation($"Cache miss for key '{cacheableQuery.CacheKey}'. Adding to cache.");
                        response = await GetResponseAndAddToCache();
                    }
                }

                if (cacheableQuery.RefreshCachedEntry)
                {
                    logger.LogInformation($"Cache refreshed for key '{cacheableQuery.CacheKey}'.");

                    await cache.RefreshAsync(cacheableQuery.CacheKey, cancellationToken);
                }

                return response;
            }
            else
            {
                return await next();
            }
        }
    }
}