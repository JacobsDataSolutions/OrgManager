// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using System;

namespace JDS.OrgManager.Application.Abstractions.Queries
{
    public interface ICacheableQuery
    {
        /// <summary>
        /// If true, then bypass the cache entirely and query the underlying data store.
        /// </summary>
        bool BypassCache { get; }

        /// <summary>
        /// Unique key to identify and store cached entries.
        /// </summary>
        string CacheKey { get; }

        /// <summary>
        /// If true, then reset the sliding expiration for the cached entry (if available) for this query.
        /// </summary>
        bool RefreshCachedEntry { get; }

        /// <summary>
        /// If true, then forcibly replace the cached entry with the value from the underlying data store.
        /// </summary>
        bool ReplaceCachedEntry { get; }

        /// <summary>
        /// If provided, then override the default sliding expiration time of the cache entry with this value.
        /// </summary>
        TimeSpan? SlidingExpiration { get; }
    }
}