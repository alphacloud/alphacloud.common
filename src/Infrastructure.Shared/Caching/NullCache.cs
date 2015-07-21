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
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    ///   Null Object for Cache
    /// </summary>
    [ExcludeFromCodeCoverage]
    class NullCache : CacheBase
    {
        /// <summary>
        ///   Default constructor.
        /// </summary>
        internal NullCache()
            : base(NullCacheHealthcheckMonitor.Instance, "<Null>")
        {
        }


        protected internal override CacheStatistics DoGetStatistics()
        {
            return new CacheStatistics(false);
        }


        protected internal override object DoGet(string key)
        {
            return null;
        }


        protected internal override void DoClear()
        {
        }


        protected internal override void DoRemove(string key)
        {
        }


        protected internal override void DoPut(string key, object value, TimeSpan ttl)
        {
        }
    }
}
