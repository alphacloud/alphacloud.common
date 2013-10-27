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
    using System.Diagnostics.CodeAnalysis;

    using JetBrains.Annotations;

    /// <summary>
    ///   Stub for healthceck monitor, reporting cache is always available.
    /// </summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public sealed class NullCacheHealthcheckMonitor : ICacheHealthcheckMonitor
    {
        /// <summary>
        ///   Null Object implementation for health-check monitor.
        /// </summary>
        public static readonly ICacheHealthcheckMonitor Instance = new NullCacheHealthcheckMonitor();


        NullCacheHealthcheckMonitor()
        {
        }

        #region ICacheHealthcheckMonitor Members

        /// <summary>
        ///   Start monitoring.
        /// </summary>
        public void Start()
        {
        }


        /// <summary>
        ///   Always reports cache is avaiable in this stub.
        /// </summary>
        public bool IsCacheAvailable
        {
            get { return true; }
        }

        #endregion
    }
}
