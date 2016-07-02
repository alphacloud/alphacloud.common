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
    using System.Collections.Generic;

    using Core.Data;

    using JetBrains.Annotations;

    /// <summary>
    ///   Cache statistics
    /// </summary>
    [Serializable]
    public class CacheStatistics
    {
        readonly List<CacheNodeStatistics> _nodes;


        /// <summary>
        ///   Initializes a new instance of the <see cref="CacheStatistics" /> class.
        /// </summary>
        /// <param name="isSuccess">
        ///   Indicates if statistics supported / retirieved succesfully.
        /// </param>
        public CacheStatistics(bool isSuccess) : this()
        {
            IsSuccess = isSuccess;
        }


        /// <summary>
        ///   Initializes a new instance of the <see cref="CacheStatistics" /> class.
        /// </summary>
        /// <param name="hitCount">The hit count.</param>
        /// <param name="getCount">The get count.</param>
        /// <param name="putCount">The put count.</param>
        /// <param name="itemCount">The item count.</param>
        /// <param name="nodes">Staistics per node.</param>
        /// <exception cref="ArgumentNullException"><paramref name="nodes" /> is <see langword="null" />.</exception>
        public CacheStatistics(long hitCount, long getCount, long putCount, long itemCount,
            [NotNull] IEnumerable<CacheNodeStatistics> nodes = null) : this()
        {
            IsSuccess = true;
            HitCount = hitCount;
            GetCount = getCount;
            PutCount = putCount;
            ItemCount = itemCount;
            if (nodes != null)
                _nodes.AddRange(nodes);
        }


        /// <summary>
        ///   Initializes a new instance of the <see cref="CacheStatistics" /> class.
        /// </summary>
        protected CacheStatistics()
        {
            _nodes = new List<CacheNodeStatistics>(8);
        }


        /// <summary>
        ///   Statistics by node.
        /// </summary>
        [NotNull]
        public IEnumerable<CacheNodeStatistics> Nodes
        {
            get { return _nodes; }
        }

        /// <summary>
        ///   Number of cache nodes.
        /// </summary>
        public int NodeCount
        {
            get { return _nodes.Count; }
        }

        /// <summary>
        ///   Gets a value indicating whether statistics request was successful.
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        ///   Cache hit count.
        /// </summary>
        public long HitCount { get; private set; }

        /// <summary>
        ///   Number of cache get requests.
        /// </summary>
        public long GetCount { get; private set; }

        /// <summary>
        ///   Number of cache put requests.
        /// </summary>
        public long PutCount { get; private set; }

        /// <summary>
        ///   Cache hit rate (in percent).
        ///   Returns <c>0</c> if not avaiable.
        /// </summary>
        public int HitRate
        {
            get
            {
                if (IsSuccess && GetCount > 0)
                {
                    return (int) ((HitCount * 10) / (GetCount / 10));
                }
                return 0;
            }
        }

        /// <summary>
        ///   Current cache item count.
        /// </summary>
        public long ItemCount { get; private set; }


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Nodes: {0}, Total items: {1}, Hit rate: {2}%".ApplyArgs(_nodes.Count, ItemCount, HitRate);
        }
    }

}
