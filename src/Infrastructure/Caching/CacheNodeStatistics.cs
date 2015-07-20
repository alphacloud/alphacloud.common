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
    using Core.Data;
    using JetBrains.Annotations;

    /// <summary>
    ///   Cache node statistics.
    /// </summary>
    [Serializable]
    [PublicAPI]
    public class CacheNodeStatistics
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheNodeStatistics"/> class.
        /// </summary>
        protected CacheNodeStatistics()
        {
        }


        /// <summary>
        ///   Initializes a new instance of the <see cref="CacheNodeStatistics" /> class.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="hitCount">The hit count.</param>
        /// <param name="getCount">The get count.</param>
        /// <param name="putCount">The put count.</param>
        /// <param name="itemCount">The item count.</param>
        public CacheNodeStatistics(string server, long hitCount, long getCount, long putCount, long itemCount)
        {
            Server = server;
            HitCount = hitCount;
            GetCount = getCount;
            PutCount = putCount;
            ItemCount = itemCount;
        }


        /// <summary>
        ///   Cache node name or address.
        /// </summary>
        public string Server { get; private set; }

        /// <summary>
        ///   Cache hit count for this node.
        /// </summary>
        public long HitCount { get; private set; }

        /// <summary>
        ///   Number of get requests sent to this node.
        /// </summary>
        public long GetCount { get; private set; }

        /// <summary>
        ///   Number of put requests sent to this node.
        /// </summary>
        public long PutCount { get; private set; }

        /// <summary>
        ///   Current number of items in cache node.
        /// </summary>
        public long ItemCount { get; private set; }

        /// <summary>
        ///   Hit rate in percent for this node.
        /// </summary>
        public int HitRate
        {
            get
            {
                if (GetCount > 0)
                {
                    return (int) ((HitCount * 100) / GetCount);
                }
                return 0;
            }
        }


        public override string ToString()
        {
            return "Node '{0}': Items: {1}, Hit rate: {2}".ApplyArgs(Server, ItemCount, HitRate);
        }
    }
}