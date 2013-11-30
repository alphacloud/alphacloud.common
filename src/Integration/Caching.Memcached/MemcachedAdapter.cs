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

using System;
using System.Collections.Generic;
using System.Linq;
using Alphacloud.Common.Core.Data;
using Alphacloud.Common.Infrastructure.Caching;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using JetBrains.Annotations;

namespace Alphacloud.Common.Caching.Memcached
{
    /// <summary>
    ///   Memcached adapter.
    /// </summary>
    class MemcachedAdapter : CacheBase
    {
        readonly IMemcachedClient _client;


        /// <summary>
        ///   Initializes a new instance of the <see cref="MemcachedAdapter" /> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="monitor">The monitor.</param>
        /// <param name="instanceName">Name of the instance.</param>
        public MemcachedAdapter(
            IMemcachedClient client, [NotNull] ICacheHealthcheckMonitor monitor, string instanceName)
            : base(monitor, instanceName)
        {
            _client = client;
        }


        protected override CacheStatistics DoGetStatistics()
        {
            var stats = _client.Stats();
            if (stats == null)
            {
                Log.Warn("Error getting statistics");
                return new CacheStatistics(false);
            }
            return new MemcachedStatsParser(stats).GetStatistics();
        }


        protected override object DoGet(string key)
        {
            return _client.Get(key);
        }


        protected override void DoRemove(string key)
        {
            _client.Remove(key);
        }


        protected override void DoPut(string key, object value, TimeSpan ttl)
        {
            if (!_client.Store(StoreMode.Set, key, value, ttl))
            {
                Log.WarnFormat("{0}: Failed to store '{1}' in cache", Name, key);
            }
        }


        protected override IDictionary<string, object> DoMultiGet(ICollection<string> keys)
        {
            var preparedKeysMap = BuildPreparedToOriginalKeyMap(keys);

            var result = new Dictionary<string, object>(keys.Count);

            var cached = _client.Get(preparedKeysMap.Keys);
            foreach (var km in preparedKeysMap)
            {
                result[km.Value] = cached.ValueOrDefault(km.Key);
            }

            // log hits and misses
            Log.Debug(m => m("{0}: MultiGet hit: {1}, miss: {2}", Name,
                new SequenceFormatter(result.Where(kv => kv.Value != null).Select(kv => kv.Key)),
                new SequenceFormatter(result.Where(kv => kv.Value == null).Select(kv => kv.Key))
                ));

            return result;
        }


        IDictionary<string, string> BuildPreparedToOriginalKeyMap(ICollection<string> keys)
        {
            var preparedkeysMap = new SortedList<string, string>(keys.Count);
            foreach (var key in keys)
            {
                preparedkeysMap[PrepareCacheKey(key)] = key;
            }
            return preparedkeysMap;
        }


        protected override void DoClear()
        {
            _client.FlushAll();
        }

        #region Overrides of CacheBase

        protected override void Dispose(bool disposing)
        {
            if (disposing && !IsDisposed)
            {
                _client.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}