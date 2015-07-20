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
    using System.Linq;
    using System.Net;
    using Enyim.Caching.Memcached;
    using Infrastructure.Caching;
    using JetBrains.Annotations;

    /// <summary>
    ///   Memcached statistics parser.
    ///   Parses STATS command result.
    /// </summary>
    class MemcachedStatsParser
    {
        readonly ServerStats _stats;

        CacheStatistics _result;


        public MemcachedStatsParser([NotNull] ServerStats stats)
        {
            if (stats == null) throw new ArgumentNullException("stats");
            _stats = stats;
        }


        /// <summary>
        ///   Gets the statistics.
        /// </summary>
        /// <returns></returns>
        public CacheStatistics GetStatistics()
        {
            if (_result == null)
            {
                ParseStats();
            }
            return _result;
        }


        void ParseStats()
        {
            var hitCount = _stats.GetValue(ServerStats.All, StatItem.GetHits);
            var putCount = _stats.GetValue(ServerStats.All, StatItem.SetCount);
            var getCount = _stats.GetValue(ServerStats.All, StatItem.GetCount);
            var totalItems = _stats.GetValue(ServerStats.All, StatItem.ItemCount);
            var nodes = _stats.GetRaw("uptime").Select(kvp => kvp.Key);
            _result = new CacheStatistics(hitCount, getCount, putCount, totalItems, nodes.Select(GetNodeStatistics));
        }


        CacheNodeStatistics GetNodeStatistics(IPEndPoint node)
        {
            var server = node.ToString();
            var getCount = _stats.GetValue(node, StatItem.GetCount);
            var hitCount = _stats.GetValue(node, StatItem.GetHits);
            var putCount = _stats.GetValue(node, StatItem.SetCount);
            var itemCount = _stats.GetValue(node, StatItem.ItemCount);
            return new CacheNodeStatistics(server, hitCount, getCount, putCount, itemCount);
        }
    }
}