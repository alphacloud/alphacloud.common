﻿#region copyright

// Copyright 2013-2015 Alphacloud.Net
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
    using System.Collections.Generic;
    using System.Linq;
    using Core.Data;
    using global::Common.Logging;
    using JetBrains.Annotations;

    /// <summary>
    ///   Basic cache class.
    /// </summary>
    [PublicAPI]
    public abstract class CacheBase : ICache, IDisposable
    {
        /// <summary>
        ///   Cache key separator.
        /// </summary>
        /// <remarks>
        ///   Pre-allocate value to avoid boxing.
        /// </remarks>
        static readonly object s_separator = '.';

        readonly ICacheHealthcheckMonitor _monitor;


        /// <summary>
        ///   Initializes a new instance of the <see cref="CacheBase" /> class.
        /// </summary>
        /// <param name="monitor">Healthcheck monitor.</param>
        /// <param name="instanceName">Name of the instance.</param>
        /// <exception cref="System.ArgumentNullException">monitor</exception>
        protected CacheBase([NotNull] ICacheHealthcheckMonitor monitor, string instanceName)
        {
            if (monitor == null) throw new ArgumentNullException("monitor");
            Log = LogManager.GetLogger(GetType());
            _monitor = monitor;
            Name = instanceName;
        }


        /// <summary>
        ///   Cache what does nothing.
        /// </summary>
        public static ICache Null { get; } = new NullCache();

        /// <summary>
        ///   Logger for this instance.
        /// </summary>
        protected ILog Log { get; }

        /// <summary>
        ///   Gets a value indicating whether this cache is available.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this cache is avaiable; otherwise, <c>false</c>.
        /// </value>
        protected bool IsAvailable
        {
            get { return _monitor.IsCacheAvailable; }
        }

        #region ICache Members

        /// <summary>
        ///   Gets cache instance name.
        /// </summary>
        /// <value>
        ///   The name.
        /// </value>
        public virtual string Name { get; private set; }


        /// <summary>
        ///   Get cache statistics
        /// </summary>
        /// <returns>Cache statistics </returns>
        public virtual CacheStatistics GetStatistics()
        {
            if (CanGet())
            {
                try
                {
                    return DoGetStatistics();
                }
                catch (Exception ex)
                {
                    Log.WarnFormat("{0}: Error getting statistics", ex, Name);
                }
            }
            return new CacheStatistics(false);
        }


        /// <summary>
        ///   Get item from cache.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>
        ///   item or <c>null</c> if no item found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   if <paramref name="key" /> is <c>null</c>
        /// </exception>
        public virtual object Get(string key)
        {
            Log.DebugFormat("{1}: Getting data of '{0}'", key, Name);
            CheckDisposed();

            if (!CanGet())
                return null;

            return SafeGetWithLogging(key);
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
            Log.Debug(m => m("{1}: Getting data for keys '{0}'", new SequenceFormatter(keys), Name));

            if (!keys.Any())
                return new Dictionary<string, object>();

            CheckDisposed();

            if (!CanGet())
            {
                var res = new Dictionary<string, object>(keys.Count);
                // build dictionary
                foreach (var key in keys)
                {
                    res[key] = null;
                }
                return res;
            }
            return DoMultiGet(keys);
        }


        /// <summary>
        ///   Clears the entire cache.
        /// </summary>
        public virtual void Clear()
        {
            Log.DebugFormat("{0}: Clearing cache", Name);
            CheckDisposed();

            if (!CanClear())
                return;

            try
            {
                DoClear();
                Log.InfoFormat("{0}: Cleared cache", Name);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("{0}: Clear Cache failed", ex, Name);
            }
        }


        /// <summary>
        ///   Remove item from cache.
        /// </summary>
        /// <param name="key">Key</param>
        public virtual void Remove([NotNull] string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            Log.DebugFormat("{1}: Removing '{0}'", key, Name);
            CheckDisposed();

            if (!CanRemove())
                return;

            try
            {
                DoRemove(PrepareCacheKey(key));
                Log.InfoFormat("{1}: Removed '{0}'", key, Name);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("{1}: Remove('{0}')", ex, key, Name);
            }
        }


        /// <summary>
        ///   Add item to cache.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Item</param>
        /// <param name="ttl">Absolute expiration timeout.</param>
        /// <exception cref="System.ArgumentNullException">key</exception>
        /// <remarks>
        ///   If <paramref name="value" /> is <c>null</c>, <see cref="Remove" /> item will be removed from cache by calling
        ///   <see cref="Remove" />.
        /// </remarks>
        public virtual void Put([NotNull] string key, object value, TimeSpan ttl)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (ttl <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("ttl", ttl, @"Cache timeout should be positive");
            }

            Log.DebugFormat("{2}: Adding '{0}', expires after {1}", key, ttl, Name);
            if (value == null)
            {
                Log.Debug("Item is null, removing");
                Remove(key);
                return;
            }

            CheckDisposed();

            if (!CanPut())
                return;

            try
            {
                DoPut(PrepareCacheKey(key), value, ttl);
                Log.InfoFormat("{1}: Added '{0}'", key, Name);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("{0}: Put('{1}')", ex, Name, key);
            }
        }


        /// <summary>
        ///   Stores multiple items to cache.
        /// </summary>
        /// <param name="data">Key-Value pairs</param>
        /// <param name="ttl">Absolute expiration timeout.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="data" /> is <c>null </c></exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="ttl" />cache timeout must be positive number.</exception>
        /// <remarks>
        ///   Items will be stored in one calls if supported by underlying cache implementation.
        /// </remarks>
        public virtual void Put([NotNull] ICollection<KeyValuePair<string, object>> data, TimeSpan ttl)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (ttl < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("ttl", ttl, @"Cache timeout must be positive");

            if (Log.IsDebugEnabled)
            {
                Log.DebugFormat("{0}: Adding {1}, expires after {2}",
                    Name, new SequenceFormatter(data.Select(kvp => kvp.Key)), ttl);
            }
            CheckDisposed();

            if (!CanPut() || !data.Any())
                return;

            var preparedKeys = new SortedList<string, object>();
            foreach (var kvp in data)
            {
                preparedKeys[PrepareCacheKey(kvp.Key)] = kvp.Value;
            }
            try
            {
                DoPutOrRemove(preparedKeys, ttl);
                Log.InfoFormat("{0}: Added {1}", Name, new SequenceFormatter(data.Select(kvp => kvp.Key)));
            }
            catch (Exception ex)
            {
                Log.WarnFormat("{0}: Put({1})", ex, Name, new SequenceFormatter(data.Select(kvp => kvp.Key)));
            }
        }

        #endregion

        object SafeGetWithLogging(string key)
        {
            try
            {
                var data = DoGet(PrepareCacheKey(key));
                LogGetSucceed(key, data);
                return data;
            }
            catch (Exception ex)
            {
                LogGetFaiulre(key, ex);
                return null;
            }
        }


        /// <summary>
        /// Logs the get operation faiulre.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="ex">The ex.</param>
        protected void LogGetFaiulre(string key, Exception ex)
        {
            Log.WarnFormat("{0}: Get('{1}')", ex, Name, key);
        }


        /// <summary>
        /// Logs successful get operation.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        protected void LogGetSucceed(string key, object data)
        {
            Log.InfoFormat("{2}: Get('{0}'): {1}", key, data != null ? "Hit" : "Miss", Name);
        }


        /// <summary>
        /// Default multi-get implementation using multiple cache requests.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <returns>Cached items as Key/Value dictionary.</returns>
        protected virtual IDictionary<string, object> DoMultiGet(ICollection<string> keys)
        {
            var res = new Dictionary<string, object>();
            foreach (var key in keys)
            {
                res[key] = SafeGetWithLogging(key);
            }

            return res;
        }


        /// <summary>
        /// Default multi-put implementation using multiple cache requests.
        /// </summary>
        /// <param name="data">Items to store in cache.</param>
        /// <param name="ttl">Absolute cache implementation.</param>
        protected virtual void DoPutOrRemove(IEnumerable<KeyValuePair<string, object>> data, TimeSpan ttl)
        {
            foreach (var kvp in data)
            {
                if (kvp.Value == null)
                    DoRemove(kvp.Key);
                else
                    DoPut(kvp.Key, kvp.Value, ttl);
            }
        }


        /// <summary>
        ///   Checks if cache is disposed.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">If already disposed.</exception>
        protected void CheckDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException("Cache '{0}' was disposed.");
        }


        /// <summary>
        ///   Retrieves statistics from underlying cache.
        /// </summary>
        /// <returns>
        ///   <see cref="CacheStatistics" />
        /// </returns>
        protected internal abstract CacheStatistics DoGetStatistics();


        /// <summary>
        ///   Prepare cache key.
        ///   Default implementation prepends key with cache name.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Prepared key.</returns>
        protected virtual string PrepareCacheKey(string key)
        {
            return string.IsNullOrEmpty(Name)
                ? key
                : string.Concat(Name, s_separator, key);
        }


        /// <summary>
        ///   Invokes Get operation on underlying cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   Value or <c>null</c>
        /// </returns>
        protected internal abstract object DoGet([NotNull] string key);


        /// <summary>
        ///   Invokes Remove opration on underlying cache.
        /// </summary>
        /// <param name="key">The key.</param>
        protected internal abstract void DoRemove([NotNull] string key);


        /// <summary>
        ///   Invokes Put operation on underlying cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">Value to put.</param>
        /// <param name="ttl">Expiration timeout.</param>
        protected internal abstract void DoPut([NotNull] string key, [NotNull] object value, TimeSpan ttl);


        /// <summary>
        ///   Clears all items in underlying cache.
        /// </summary>
        protected internal abstract void DoClear();


        /// <summary>
        ///   Determines whether Get operation can be executed for cache.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if can get from cache; otherwise, <c>false</c>.
        /// </returns>
        protected bool CanGet()
        {
            if (!IsAvailable)
            {
                Log.DebugFormat("{0} is not avaiable, returning null", Name);
                return false;
            }
            return true;
        }


        /// <summary>
        ///   Determines whether Put operation can be executed for cache.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if can put data to cache; otherwise, <c>false</c>.
        /// </returns>
        protected bool CanPut()
        {
            if (!IsAvailable)
            {
                Log.DebugFormat("{0} is not available, skipping Put()", Name);
                return false;
            }
            return true;
        }


        /// <summary>
        ///   Determines whether Remove operation can be executed for cache.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if data can be removed from cache; otherwise, <c>false</c>.
        /// </returns>
        protected bool CanRemove()
        {
            if (!IsAvailable)
            {
                Log.DebugFormat("{0} is not available, skipping Remove()", Name);
                return false;
            }
            return true;
        }


        /// <summary>
        ///   Determines whether cache is available and can be cleared.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if cache can be cleared; otherwise, <c>false</c>.
        /// </returns>
        protected bool CanClear()
        {
            if (!IsAvailable)
            {
                Log.DebugFormat("{0} is not avaiable, skipping Clear()", Name);
                return false;
            }

            return true;
        }


        /// <summary>
        ///   Logs cache hits and cache misses (in debug mode).
        /// </summary>
        /// <param name="result">Cache Key-Value map.</param>
        protected void LogMultiGetStatistics(Dictionary<string, object> result)
        {
            if (!Log.IsDebugEnabled)
                return;

            Log.DebugFormat("{0}: MultiGet hit: {1}, miss: {2}", Name,
                new SequenceFormatter(result.Where(kv => kv.Value != null).Select(kv => kv.Key)),
                new SequenceFormatter(result.Where(kv => kv.Value == null).Select(kv => kv.Key))
                );
        }

        #region Implementation of IDisposable

        /// <summary>
        ///   Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisposed { get; private set; }


        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        ///   Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///   <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                IsDisposed = true;
            }
        }

        #endregion
    }
}