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
    using System.Globalization;

    using Alphacloud.Common.Core.Data;

    using JetBrains.Annotations;

    using global::Common.Logging;

    /// <summary>
    ///   Composite cache.
    ///   Consists of 2 caches: local (short-term) and remote (long-term).
    /// </summary>
    public class CompositeCache : ICache
    {
        /// <summary>
        ///   Predefined name for local cache.
        ///   User with statistics.
        /// </summary>
        public const string LocalCacheName = "LocalCache";

        static readonly ILog s_log = LogManager.GetCurrentClassLogger();
        readonly bool _devMode;

        readonly ICache _localCache;
        readonly ICache _remoteCache;
        string _machineName;


        /// <summary>
        ///   Initializes a new instance of the <see cref="CompositeCache" /> class.
        /// </summary>
        /// <param name="localCache">The local cache.</param>
        /// <param name="remoteCache">The remote cache.</param>
        /// <param name="localCacheTimeout">The local cache timeout.</param>
        /// <param name="devMode">
        ///   if set to <c>true</c> [debug mode].
        /// </param>
        /// <exception cref="System.ArgumentNullException">localCache is null</exception>
        /// <exception cref="System.ArgumentNullException">remoteCache is null</exception>
        public CompositeCache(
            [NotNull] ICache localCache, [NotNull] ICache remoteCache, TimeSpan localCacheTimeout,
            bool devMode = false)
        {
            if (localCache == null)
            {
                throw new ArgumentNullException("localCache");
            }
            if (remoteCache == null)
            {
                throw new ArgumentNullException("remoteCache");
            }

            _localCache = localCache;
            _remoteCache = remoteCache;
            LocalCacheTimeout = localCacheTimeout;
            s_log.DebugFormat(CultureInfo.InvariantCulture, "Local cache timeout={0:0.00} sec",
                localCacheTimeout.TotalSeconds);
            _devMode = devMode;
            if (_devMode)
            {
                s_log.Info(
                    "Running in development mode, all cache keys will be prefixed to ensure they are unique during current application session.");
            }
        }


        /// <summary>
        ///   Gets the local cache timeout.
        /// </summary>
        /// <value>
        ///   The local cache timeout.
        /// </value>
        public TimeSpan LocalCacheTimeout { get; private set; }

        /// <summary>
        ///   Calculate cache key prefix for use in Dev Mode.
        ///   Prefix will change each time application restarted.
        /// </summary>
        internal string CacheKeyPrefix
        {
            get
            {
                if (string.IsNullOrEmpty(_machineName))
                {
                    lock (this)
                    {
                        if (string.IsNullOrEmpty(_machineName))
                        {
                            try
                            {
                                _machineName = "{0}({1:00}).".ApplyArgs(Environment.MachineName, DateTime.Now.Second);
                            }
                            catch (Exception ex)
                            {
                                s_log.Warn("Error getting computer name, using GUID", ex);
                                _machineName = Guid.NewGuid().ToString();
                            }
                            s_log.InfoFormat("Using '{0}' as cache key prefix",
                                _machineName);
                        }
                    }
                }
                return _machineName;
            }
        }

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
            var remoteStats = _remoteCache.GetStatistics();
            var localStats = _localCache.GetStatistics();
            if (localStats.IsSuccess)
            {
                remoteStats.Nodes.Add(
                    new CacheNodeStatistics(LocalCacheName, localStats.HitCount, localStats.GetCount,
                        localStats.PutCount, localStats.ItemCount));
            }
            return remoteStats;
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
                s_log.DebugFormat("Loading '{0}' from remote cache...", key);
                item = _remoteCache.Get(cacheKey);
                if (item != null)
                {
                    _localCache.Put(cacheKey, item, LocalCacheTimeout);
                }
            }
            else
            {
                s_log.DebugFormat("Returning local copy of '{0}'", key);
            }
            return item;
        }


        /// <summary>
        ///   Clears the entire cache.
        /// </summary>
        public void Clear()
        {
            _localCache.Clear();
            _remoteCache.Clear();
        }


        /// <summary>
        ///   Remove item from cache.
        /// </summary>
        /// <param name="key">Key</param>
        public void Remove(string key)
        {
            key = FormatKey(key);
            _localCache.Remove(key);
            _remoteCache.Remove(key);
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
            _localCache.Put(key, value, (ttl < LocalCacheTimeout) ? ttl : LocalCacheTimeout);
            _remoteCache.Put(key, value, ttl);
        }


        string FormatKey(string key)
        {
            return _devMode
                ? string.Concat(CacheKeyPrefix, key)
                : key;
        }
    }
}
