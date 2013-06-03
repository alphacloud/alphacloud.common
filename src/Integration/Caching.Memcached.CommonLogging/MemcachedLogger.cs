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

namespace Alphacloud.Common.Caching.Memcached.CommonLogging
{
    using System;
    using System.Globalization;
    using Enyim.Caching;
    using Enyim.Caching.Memcached;
    using JetBrains.Annotations;
    using ILog = global::Common.Logging.ILog;
    using LogManager = global::Common.Logging.LogManager;

    /// <summary>
    ///   Memcached logger helper
    /// </summary>
    public static class MemcachedLogger
    {
        static readonly MemcachedLogFactory s_factory = new MemcachedLogFactory();
        static readonly ILog s_memcachedLog = LogManager.GetLogger<MemcachedClient>();

        /// <summary>
        ///   <see cref="MemcachedLogFactory" /> instance.
        /// </summary>
        public static ILogFactory Factory
        {
            get { return s_factory; }
        }


        /// <summary>
        ///   Subscribe and log cache node failures.
        /// </summary>
        /// <param name="client">Memcached client.</param>
        public static void StartLogingNodeFailures([NotNull] IMemcachedClient client)
        {
            if (client == null) throw new ArgumentNullException("client");
            client.NodeFailed += ClientOnNodeFailed;
        }


        /// <summary>
        ///   Unsubscribe from cache node failures.
        /// </summary>
        /// <param name="client">Memcached client.</param>
        public static void StopLogingNodeFailures([NotNull] IMemcachedClient client)
        {
            if (client == null) throw new ArgumentNullException("client");
            client.NodeFailed -= ClientOnNodeFailed;
        }


        static void ClientOnNodeFailed(IMemcachedNode memcachedNode)
        {
            if (memcachedNode == null)
                return;
            if (!memcachedNode.IsAlive)
            {
                s_memcachedLog.WarnFormat(CultureInfo.InvariantCulture, "Node {0} is down.", memcachedNode.EndPoint);
            }
        }
    }
}