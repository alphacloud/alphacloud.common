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

namespace Alphacloud.Common.Core.Data
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using JetBrains.Annotations;

    /// <summary>
    ///   Serializer interface.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        ///   Gets amount of memory allicated by underlying data structures.
        /// </summary>
        int AllocatedMemorySize { get; }


        /// <summary>
        ///   Serialize object.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>Serialized object</returns>
        [NotNull]
        byte[] Serialize([NotNull] object obj);


        /// <summary>
        ///   Deserializes object.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>Deserialized object.</returns>
        /// <exception cref="ArgumentNullException"> if <paramref name="buffer"/> is <c>null</c>.</exception>
        [CanBeNull]
        object Deserialize([NotNull] byte[] buffer);
    }

    /// <summary>
    ///   Serialize object's text representation (.ToString()) as UTF8 encoded string.
    /// </summary>
    /// <remarks>
    ///   Used for debugging.
    ///   Deserialized object is always string.
    /// </remarks>
    public class StringSerializer : ISerializer
    {
        #region ISerializer Members

        //
        /// <summary>
        /// Gets the size of the allocated memory.
        /// </summary>
        /// <value>
        /// The size of the allocated memory.
        /// </value>
        public int AllocatedMemorySize => 1;


        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">obj</exception>
        public byte[] Serialize(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            return Encoding.UTF8.GetBytes(obj.ToString());
        }


        /// <summary>
        ///   Reads string from buffer using UTF8 encoding.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>Deserialized <see cref="string"/>.</returns>
        /// <exception cref="ArgumentNullException"> if <paramref name="buffer"/> is <c>null</c>.</exception>
        public object Deserialize(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (buffer.Length == 0)
                return string.Empty;

            return Encoding.UTF8.GetString(buffer);
        }

        #endregion
    }

    /// <summary>
    ///   Serialize object using <see cref="BinaryFormatter" />.
    /// </summary>
    public class CompactBinarySerializer : CompactBinarySerializerBase, IDisposable
    {
        MemoryStream _stream;

        #region IDisposable Members

        /// <summary>
        ///   Dispose internal memory stream.
        /// </summary>
        public void Dispose()
        {
            if (_stream == null) return;

            _stream.Dispose();
            _stream = null;
        }

        #endregion

        #region ISerializer Members

        /// <summary>
        /// Gets the size of the allocated memory.
        /// </summary>
        /// <value>
        /// The size of the allocated memory.
        /// </value>
        public override int AllocatedMemorySize
        {
            get { return AcquireStream().Capacity; }
        }

        #endregion

        /// <summary>
        /// Acquires the stream.
        /// </summary>
        /// <returns></returns>
        protected override MemoryStream AcquireStream()
        {
            return _stream = (_stream ?? new MemoryStream());
        }


        /// <summary>
        /// Releases the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        protected override void ReleaseStream(MemoryStream stream)
        {
            // do nothing
        }
    }
}