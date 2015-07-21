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
    using System.Runtime.Caching;
    using JetBrains.Annotations;

    /// <summary>
    ///   .NET MemoryCache factory
    /// </summary>
    [PublicAPI]
    public class MemoryCacheFactory : CacheFactoryBase
    {
        #region Overrides of CacheFactoryBase

        public override void Initialize()
        {
            Log.Info("Initialized");
        }


        /// <summary>
        ///   Creates <see cref="MemoryCacheAdapter" /> for <seealso cref="MemoryCache" />
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        protected internal override ICache CreateCache(string instance)
        {
            if (!IsEnabled)
            {
                return base.CreateCache(instance);
            }

            var memCache = new MemoryCache(instance);
            var adapter = new MemoryCacheAdapter(memCache, NullCacheHealthcheckMonitor.Instance, instance);
            return adapter;
        }

        #endregion
    }
}