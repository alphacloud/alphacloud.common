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

namespace Alphacloud.Common.Caching.Memcached.CommonLogging
{
    using System;
    using Enyim.Caching;
    using LogManager = global::Common.Logging.LogManager;

    /// <summary>
    ///   Logger factory for memcached.
    /// </summary>
    public class MemcachedLogFactory : ILogFactory
    {
        #region ILogFactory Members

        /// <summary>
        ///   Get logger by name.
        /// </summary>
        /// <param name="name">Logger name.</param>
        /// <returns>
        ///   <see cref="ILog" />
        /// </returns>
        public ILog GetLogger(string name)
        {
            return new MemcachedCommonLoggingAdapter(LogManager.GetLogger(name));
        }


        /// <summary>
        ///   Get logger by type.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>
        ///   <see cref="ILog" />
        /// </returns>
        public ILog GetLogger(Type type)
        {
            return new MemcachedCommonLoggingAdapter(LogManager.GetLogger(type));
        }

        #endregion
    }
}