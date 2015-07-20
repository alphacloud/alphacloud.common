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
    using System.Runtime.Serialization.Formatters;
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
        int MemoryAllocated { get; }


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
        /// <returns></returns>
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
        public int MemoryAllocated
        {
            get { return 1; }
        }


        public byte[] Serialize(object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            return Encoding.UTF8.GetBytes(obj.ToString());
        }


        public object Deserialize(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (buffer.Length == 0)
                return string.Empty;

            return Encoding.UTF8.GetString(buffer);
        }

        #endregion
    }

    public abstract class CompactBinarySerializerBase
    {
        readonly BinaryFormatter _formatter;


        internal protected CompactBinarySerializerBase([NotNull] BinaryFormatter formatter)
        {
            if (formatter == null) throw new ArgumentNullException("formatter");
            _formatter = formatter;
        }


        protected CompactBinarySerializerBase() : this(new BinaryFormatter {
            AssemblyFormat = FormatterAssemblyStyle.Simple,
            TypeFormat = FormatterTypeStyle.TypesWhenNeeded
        })
        {
        }


        public abstract int MemoryAllocated { get; }


        public byte[] Serialize([NotNull] object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            var stream = AcquireStream();

            try
            {
                stream.Position = 0;
                _formatter.Serialize(stream, obj);
                stream.SetLength(stream.Position);
                return stream.ToArray();
            }
            finally
            {
                ReleaseStream(stream);
            }
        }


        /// <summary>
        ///   Acquires the stream.
        /// </summary>
        /// <returns></returns>
        protected abstract MemoryStream AcquireStream();


        /// <summary>
        ///   Releases the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        protected abstract void ReleaseStream(MemoryStream stream);

        /// <summary>
        /// Deserializes data.
        /// </summary>
        /// <param name="buffer">Binary buffer.</param>
        /// <returns>Deserialized object.</returns>
        /// <exception cref="System.ArgumentNullException">buffer is <c>null</c></exception>
        /// <remarks>This method does not use AcquireStream/ReleaseStream because memory buffer already exists.</remarks>
        public object Deserialize([NotNull] byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");

            if (buffer.Length == 0)
                return null;
            using (var ms = new MemoryStream(buffer))
            {
                return _formatter.Deserialize(ms);
            }
        }
    }

    /// <summary>
    ///   Serialize object using <see cref="BinaryFormatter" />.
    /// </summary>
    public class CompactBinarySerializer : CompactBinarySerializerBase, ISerializer, IDisposable
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

        public override int MemoryAllocated
        {
            get { return AcquireStream().Capacity; }
        }

        #endregion

        protected override MemoryStream AcquireStream()
        {
            return _stream = (_stream ?? new MemoryStream());
        }


        protected override void ReleaseStream(MemoryStream stream)
        {
            // do nothing
        }
    }
}