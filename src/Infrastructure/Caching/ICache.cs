#region copyright

// Copyright 2013 Alphacloud.Net
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

#endregion

namespace Alphacloud.Common.Infrastructure.Caching
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    ///   Base interface for cache.
    /// </summary>
    public interface ICache
    {
        /// <summary>
        ///   Gets name of the cache.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///   Get item from cache.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>
        ///   item or <c>null</c> if no item found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   if <paramref name="key" /> is null.
        /// </exception>
        object Get([NotNull] string key);

        /// <summary>
        ///   Clears the entire cache.
        /// </summary>
        void Clear();

        /// <summary>
        ///   Remove item from cache.
        /// </summary>
        /// <param name="key">Key</param>
        void Remove([NotNull] string key);

        /// <summary>
        ///   Add item to cache.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Item</param>
        /// <param name="ttl">Absolute expiration timeout.</param>
        /// <exception cref="ArgumentNullException">
        ///   if <paramref name="key" />is <c>null</c>
        /// </exception>
        /// <remarks>
        ///   If <paramref name="value" /> is <c>null</c>, <see cref="Remove" /> item will be removed from cache by calling
        ///   <see cref="Remove" />.
        /// </remarks>
        void Put([NotNull] string key, [CanBeNull] object value, TimeSpan ttl);

        /// <summary>
        ///   Get cache statistics
        /// </summary>
        /// <returns></returns>
        [NotNull]
        CacheStatistics GetStatistics();
    }
}
