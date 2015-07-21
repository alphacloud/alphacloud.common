#region copyright

// Copyright 2013-2015 Alphacloud.Net
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

namespace Alphacloud.Common.Caching.Redis
{
    using System;
    using System.IO;
    using Core.Data;
    using JetBrains.Annotations;
    using Microsoft.IO;

    /// <summary>
    ///   Compact binary serializer which uses recyclable memory streams to reduce memory allocations during serialization.
    /// </summary>
    /// <seealso cref="RecyclableMemoryStream" />
    /// <seealso cref="RecyclableMemoryStreamManager" />
    class CompactSerializerWithRecyclableMemoryStream : CompactBinarySerializerBase
    {
        readonly RecyclableMemoryStreamManager _streamManager;


        /// <summary>
        ///   Initializes a new instance of the <see cref="CompactSerializerWithRecyclableMemoryStream" /> class.
        /// </summary>
        /// <param name="streamManager">The stream manager.</param>
        /// <exception cref="ArgumentNullException"><paramref name="streamManager" /> is <see langword="null" />.</exception>
        public CompactSerializerWithRecyclableMemoryStream([NotNull] RecyclableMemoryStreamManager streamManager)
        {
            if (streamManager == null) throw new ArgumentNullException("streamManager");
            _streamManager = streamManager;
        }


        public override int MemoryAllocated
        {
            get { return 1; }
        }


        /// <summary>
        ///   Acquires the stream.
        /// </summary>
        /// <returns></returns>
        protected override MemoryStream AcquireStream()
        {
            return _streamManager.GetStream("CompactSerializerWithRecyclableMemoryStream");
        }


        /// <summary>
        ///   Releases the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        protected override void ReleaseStream(MemoryStream stream)
        {
            stream.Dispose();
        }
    }
}