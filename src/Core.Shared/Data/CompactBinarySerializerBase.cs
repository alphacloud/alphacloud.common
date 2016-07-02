namespace Alphacloud.Common.Core.Data
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters;
    using System.Runtime.Serialization.Formatters.Binary;
    using JetBrains.Annotations;

    /// <summary>
    /// Binary-formatter based serializer.
    /// </summary>
    public abstract class CompactBinarySerializerBase: ISerializer
    {
        readonly BinaryFormatter _formatter;


        /// <summary>
        /// Initializes a new instance of the <see cref="CompactBinarySerializerBase"/> class.
        /// </summary>
        /// <param name="formatter">The formatter.</param>
        /// <exception cref="System.ArgumentNullException">formatter</exception>
        protected internal CompactBinarySerializerBase([NotNull] BinaryFormatter formatter)
        {
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));
            _formatter = formatter;
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="CompactBinarySerializerBase"/> class.
        /// </summary>
        protected CompactBinarySerializerBase() : this(new BinaryFormatter {
            AssemblyFormat = FormatterAssemblyStyle.Simple,
            TypeFormat = FormatterTypeStyle.TypesWhenNeeded
        })
        {
        }


        /// <summary>
        /// Gets the allocated memory size.
        /// </summary>
        /// <value>
        /// The memory allocated.
        /// </value>
        public abstract int AllocatedMemorySize { get; }


        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public byte[] Serialize([NotNull] object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
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
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            if (buffer.Length == 0)
                return null;
            using (var ms = new MemoryStream(buffer))
            {
                return _formatter.Deserialize(ms);
            }
        }
    }
}