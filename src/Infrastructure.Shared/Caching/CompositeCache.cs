﻿#region copyright

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

using System;
using System.Collections.Generic;
using System.Linq;
using Alphacloud.Common.Core.Data;
using Common.Logging;
using JetBrains.Annotations;

namespace Alphacloud.Common.Infrastructure.Caching
{
    using System.Globalization;
    using System.Threading.Tasks;

    /// <summary>
    ///   Composite cache.
    ///   Aggregates two caches: local (short-term) and backing (long-term).
    /// </summary>
    public class CompositeCache : ICache
    {
        /// <summary>
        ///   Predefined name for local cache.
        ///   Used for statistics and IOC registration.
        /// </summary>
        public const string LocalCacheInstanceName = "LocalCache";

        /// <summary>
        ///   Predefined name of backing cache.
        ///   Used for IOC registration.
        /// </summary>
        public const string BackingCacheInstanceName = "BackingCache";

        static readonly ILog s_log = LogManager.GetLogger<CompositeCache>();
        readonly ICache _backingCache;
        readonly bool _devMode;
        readonly ICache _localCache;
        readonly ILocalCacheTimeoutStrategy _localTimeoutStrategy;

        string _prefix;


        /// <summary>
        ///   Initializes a new instance of the <see cref="CompositeCache" /> class.
        /// </summary>
        /// <param name="localCache">The local cache.</param>
        /// <param name="backingCache">The backing cache.</param>
        /// <param name="localTimeoutStrategy">The local cache timeout calculation strategy.</param>
        /// <param name="devMode">
        ///   Development mode.
        ///   if set to <c>true</c> cache keys will be prefixed with unque value to prevent collisions whan using same cache by
        ///   development team.
        /// </param>
        /// <exception cref="System.ArgumentNullException">localCache is null</exception>
        /// <exception cref="System.ArgumentNullException">backingCache is null</exception>
        /// <remarks>
        ///   When <c>devMode</c> is on, cache keys are prefixed with machine name and random number. This prevents cache
        ///   collisions when using same cache server by development team.
        ///   Also this allows to start with empty cache by simply restarting application.
        /// </remarks>
        public CompositeCache(
            [NotNull] ICache localCache, [NotNull] ICache backingCache,
            [NotNull] ILocalCacheTimeoutStrategy localTimeoutStrategy,
            bool devMode = false)
        {
            if (localCache == null)
            {
                throw new ArgumentNullException("localCache");
            }
            if (backingCache == null)
            {
                throw new ArgumentNullException("backingCache");
            }
            if (localTimeoutStrategy == null) throw new ArgumentNullException("localTimeoutStrategy");

            _localCache = localCache;
            _backingCache = backingCache;
            _localTimeoutStrategy = localTimeoutStrategy;
            _devMode = devMode;
            if (_devMode)
            {
                s_log.Info(
                    "Running in development mode, all cache keys will be prefixed to ensure they are unique during current application session.");
            }
        }

        #region ICache Members

        /// <summary>
        ///   Cache name.
        /// </summary>
        public string Name { get; private set; }


        /// <summary>
        ///   Get cache statistics
        /// </summary>
        /// <returns></returns>
        public CacheStatistics GetStatistics()
        {
            var backingCacheStats = _backingCache.GetStatistics();
            var localCacheStats = _localCache.GetStatistics();
            if (localCacheStats.IsSuccess)
            {
                var nodes = new List<CacheNodeStatistics>(backingCacheStats.NodeCount + 1) {
                    new CacheNodeStatistics(LocalCacheInstanceName, localCacheStats.HitCount, localCacheStats.GetCount,
                        localCacheStats.PutCount, localCacheStats.ItemCount)
                };
                nodes.AddRange(backingCacheStats.Nodes);

                return new CacheStatistics(backingCacheStats.HitCount+localCacheStats.HitCount, backingCacheStats.GetCount+localCacheStats.GetCount, 
                    backingCacheStats.PutCount, // items always added to both cached, so use backing cache parameter
                    backingCacheStats.ItemCount+localCacheStats.ItemCount,
                    nodes);
            }
            return backingCacheStats;
        }


        /// <summary>
        ///   Get item from cache.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>
        ///   item or <c>null</c> if no item found.
        /// </returns>
        public object Get(string key)
        {
            string cacheKey = FormatKey(key);
            object item = _localCache.Get(cacheKey);
            if (item == null)
            {
                s_log.DebugFormat("Loading '{0}' from backing cache...", key);
                item = _backingCache.Get(cacheKey);
                if (item != null)
                {
                    _localCache.Put(cacheKey, item, _localTimeoutStrategy.GetLocalTimeout(TimeSpan.Zero));
                }
            }
            else
            {
                s_log.DebugFormat("Returning local copy of '{0}'", key);
            }
            return item;
        }


        /// <summary>
        /// Gets multiple items from cache.
        /// </summary>
        /// <remarks>
        /// Items will be retrieved in one call if supported by underlying cache implementation. If not, multiple requests will be made.
        /// </remarks>
        /// <param name="keys">List of item keys to retrieve.</param>
        /// <returns>Key/Value dictionary. Missing items will have <c>null</c> as value.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="keys"/> is <c>null</c>.</exception>
        public IDictionary<string, object> Get([NotNull] ICollection<string> keys)
        {
            if (keys == null) throw new ArgumentNullException("keys");
            var localData = _localCache.Get(keys);

            var missingKeys = keys.Where(k => localData.ValueOrDefault(k) == null).ToArray();
            if (!missingKeys.Any()) return localData;

            var remoteData = _backingCache.Get(missingKeys);
            foreach (var kv in remoteData)
            {
                // update local cache: make sure no obsolete data stored locally
                if (kv.Value != null)
                    _localCache.Put(kv.Key, kv.Value, _localTimeoutStrategy.GetLocalTimeout(TimeSpan.Zero));
                else
                    _localCache.Remove(kv.Key);

                localData[kv.Key] = kv.Value;
            }
            return localData;
        }


        public void Put([NotNull] ICollection<KeyValuePair<string, object>> data, TimeSpan ttl)
        {
            if (data == null) throw new ArgumentNullException("data");
            if(!data.Any())
                return;
            try
            {
                // prepare data for parallel upload
                var local = new List<KeyValuePair<string, object>>(data.Count);
                var remote = new List<KeyValuePair<string, object>>(data.Count);
                foreach (var pair in data)
                {
                    var formattedKey = FormatKey(pair.Key);
                    local.Add(new KeyValuePair<string, object>(formattedKey, pair.Value));
                    remote.Add(new KeyValuePair<string, object>(formattedKey, pair.Value));
                }

                var updateTasks = new[] {
                    Task.Factory.StartNew(() => _backingCache.Put(data, ttl)),
                    Task.Factory.StartNew(() => _localCache.Put(data, _localTimeoutStrategy.GetLocalTimeout(ttl))),    
                };

                Task.WaitAll(updateTasks);
            }
            catch (Exception ex)
            {
                s_log.WarnFormat(CultureInfo.InvariantCulture, "Error storing data {0}", ex,
                    new SequenceFormatter(data.Select(p => p.Key)));
            }
        }


        /// <summary>
        ///   Clears the entire cache.
        /// </summary>
        public void Clear()
        {
            _localCache.Clear();
            _backingCache.Clear();
        }


        /// <summary>
        ///   Remove item from cache.
        /// </summary>
        /// <param name="key">Key</param>
        public void Remove(string key)
        {
            key = FormatKey(key);
            _localCache.Remove(key);
            _backingCache.Remove(key);
        }


        /// <summary>
        ///   Add item to cache.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Item</param>
        /// <param name="ttl">Absolute expiration timeout.</param>
        /// <remarks>
        ///   If <paramref name="value" /> is <c>null</c>, <see cref="Remove" /> item will be removed from cache by calling
        ///   <see cref="Remove" />
        ///   .
        /// </remarks>
        public void Put(string key, object value, TimeSpan ttl)
        {
            key = FormatKey(key);
            _localCache.Put(key, value, _localTimeoutStrategy.GetLocalTimeout(ttl));
            _backingCache.Put(key, value, ttl);
        }

        #endregion

        /// <summary>
        ///   Calculate cache key prefix for use in Dev Mode.
        ///   Prefix will change each time application restarted.
        /// </summary>
        internal string GetCacheKeyPrefix()
        {
            if (!string.IsNullOrEmpty(_prefix)) return _prefix;

            lock (this)
            {
                if (string.IsNullOrEmpty(_prefix))
                {
                    try
                    {
                        _prefix = "{0}-{1:00}.".ApplyArgs(Environment.MachineName, DateTime.Now.Second);
                    }
                    catch (Exception ex)
                    {
                        s_log.Warn("Error retrieving computer name, using GUID", ex);
                        _prefix = Guid.NewGuid().ToString();
                    }
                    s_log.InfoFormat("Using '{0}' as cache key prefix", _prefix);
                }
            }
            return _prefix;
        }


        string FormatKey(string key)
        {
            return _devMode
                ? string.Concat(GetCacheKeyPrefix(), key)
                : key;
        }
    }
}