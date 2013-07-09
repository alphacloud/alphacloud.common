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

namespace Alphacloud.Common.Caching.Memcached
{
    using System;
    using CommonLogging;
    using Core.Data;
    using Enyim.Caching;
    using Infrastructure.Caching;

    /// <summary>
    ///   Cache factory for memcached.
    /// </summary>
    public class MemcachedFactory : CacheFactoryBase
    {
        static readonly Lazy<MemcachedLogFactory> s_memcachedLogFactory = new Lazy<MemcachedLogFactory>();
        readonly MemcachedAvailabilityMonitor _availabilityMonitor = new MemcachedAvailabilityMonitor();

        /// <summary>
        /// Initialize factory.
        /// </summary>
        public override void Initialize()
        {
            if (!s_memcachedLogFactory.IsValueCreated) // assign log factory only once.
                LogManager.AssignFactory(s_memcachedLogFactory.Value);
            Log.Info("Initialized");
        }

        /// <summary>
        /// Create new named memcached adapter.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        protected override ICache CreateCache(string instance)
        {
            var cache = base.CreateCache(instance);
            if (cache != null)
                return cache;
            // use default configuration
            if (string.IsNullOrEmpty(instance))
                return new MemcachedAdapter(new MemcachedClient(), _availabilityMonitor, DefaultInstanceName);

            var configPath = "enyim.com/{0}".ApplyArgs(instance);
            Log.InfoFormat("Using custom configuration from '{0}", configPath);
            return new MemcachedAdapter(new MemcachedClient(configPath), _availabilityMonitor, instance);
        }
    }
}