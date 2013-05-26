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
    using System.Diagnostics;
    using Core.Utils;

    /// <summary>
    ///   Cache item with last updated date attached.
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    [Serializable]
    [DebuggerDisplay("{Item} @{LastModifiedUtc}")]
    public class Cached<T>
    {
        /// <summary>
        ///   Create cached item using current date as last modification date-time.
        /// </summary>
        /// <param name="item">Data item.</param>
        public Cached(T item)
        {
            Item = item;
            LastModifiedUtc = Clock.CurrentTimeUtc();
        }


        /// <summary>
        ///   Create cached item.
        /// </summary>
        /// <param name="lastModifiedUtc">Last modification date (UTC).</param>
        /// <param name="item">Data item.</param>
        public Cached(DateTime lastModifiedUtc, T item = default(T))
        {
            LastModifiedUtc = lastModifiedUtc;
            Item = item;
        }


        /// <summary>
        ///   Last data modification time.
        /// </summary>
        public DateTime LastModifiedUtc { get; private set; }

        /// <summary>
        ///   Gets or sets data item.
        /// </summary>
        /// <value>
        ///   Data item.
        /// </value>
        public T Item { get; private set; }
    }
}