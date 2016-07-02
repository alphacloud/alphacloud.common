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

namespace Alphacloud.Common.Infrastructure.Caching.MemoryCache
{
    using System;
    using System.Linq;
    using System.Runtime.Caching;

    using Alphacloud.Common.Core.Utils;

    using JetBrains.Annotations;

    /// <summary>
    ///   .Net Memory cache adapter.
    /// </summary>
    public class MemoryCacheAdapter : CacheBase
    {
        readonly ObjectCache _cache;


        /// <summary>
        ///   Initializes a new instance of the <see cref="MemoryCacheAdapter" /> class.
        /// </summary>
        /// <param name="cache">The cache.</param>
        /// <param name="monitor">The monitor.</param>
        /// <param name="instanceName">Name of the instance.</param>
        /// <exception cref="System.ArgumentNullException">cache</exception>
        public MemoryCacheAdapter(
            [NotNull] ObjectCache cache, [NotNull] ICacheHealthcheckMonitor monitor, string instanceName)
            : base(monitor, instanceName)
        {
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }
            _cache = cache;
        }

        #region Overrides of CacheBase

        protected internal override object DoGet(string key)
        {
            return _cache.Get(key);
        }


        protected internal override void DoRemove(string key)
        {
            _cache.Remove(key);
        }


        protected internal override void DoPut(string key, object value, TimeSpan ttl)
        {
            _cache.Set(key, value, DateTimeOffset.UtcNow.Add(ttl));
        }


        protected internal override void DoClear()
        {
            Log.DebugFormat("Dump of cache {0}", Name);
            var entries = _cache.ToArray();
            foreach (var item in entries)
            {
                Log.DebugFormat("{0}: {1}", item.Key, item.Value);
                _cache.Remove(item.Key);
            }
            Log.DebugFormat("End dump of {0}", Name);
        }


        protected internal override CacheStatistics DoGetStatistics()
        {
            Log.Info("GetStatistics is not supported");
            return new CacheStatistics(false);
        }


        protected override string PrepareCacheKey(string key)
        {
            // no need to prefix key with cache name, since we using separate cache instances
            return key;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && !IsDisposed)
            {
                Disposer.TryDispose(_cache);
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
