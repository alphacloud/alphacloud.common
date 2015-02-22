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

    public interface ISerializer
    {
        int MemoryAllocated { get; }
        byte[] Serialize([NotNull] object obj);
        object Deserialize([NotNull] byte[] buffer);
    }

    public class StringSerializer : ISerializer
    {
        #region ISerializer Members

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

    public class CompactBinarySerializer : IDisposable, ISerializer
    {
        readonly BinaryFormatter _formatter;
        readonly MemoryStream _stream;


        public CompactBinarySerializer()
        {
            _formatter = new BinaryFormatter {
                AssemblyFormat = FormatterAssemblyStyle.Simple,
                TypeFormat = FormatterTypeStyle.TypesWhenNeeded
            };
            _stream = new MemoryStream();
        }

        #region ISerializer Members

        public int MemoryAllocated
        {
            get { return _stream.Capacity; }
        }


        public byte[] Serialize([NotNull] object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            _stream.Position = 0;
            _formatter.Serialize(_stream, obj);
            _stream.SetLength(_stream.Position);
            return _stream.ToArray();
        }


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

        #endregion

        #region IDisposable Members

        /// <summary>
        ///   Dispose internal memory stream.
        /// </summary>
        public void Dispose()
        {
            _stream.Dispose();
        }

        #endregion
    }
}