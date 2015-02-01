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

    using global::Common.Logging;

    /// <summary>
    ///   Null cache factory.
    /// </summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public sealed class NullCacheFactory : ICacheFactory

    {
        static readonly ILog s_log = LogManager.GetLogger<NullCacheFactory>();

        #region ICacheFactory Members

        /// <summary>
        ///   Returns NullCache implementation.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public ICache GetCache(string instance = null)
        {
            return CacheBase.Null;
        }


        /// <summary>
        ///   Initialization.
        /// </summary>
        public void Initialize()
        {
            s_log.Info("Initialize cache");
        }

        #endregion
    }
}
