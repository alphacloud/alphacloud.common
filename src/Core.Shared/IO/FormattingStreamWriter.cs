#region copyright

// Copyright 2013-2014 Alphacloud.Net
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

namespace Alphacloud.Common.Core.IO
{
    using System;
    using System.IO;
    using System.Text;
    using JetBrains.Annotations;

    /// <summary>
    ///   Implements StreamWriter for writing characters to Stream in particular encoding and with Particular IFormatProvider.
    /// </summary>
    [PublicAPI]
    public class FormattingStreamWriter : StreamWriter
    {
        readonly IFormatProvider _formatProvider;


        /// <summary>
        ///   Initializes a new instance of the <see cref="FormattingStreamWriter" /> class.
        /// </summary>
        /// <param name="path">The complete file path to write to.</param>
        /// <param name="append">
        ///   Determines whether data is to be appended to the file. If the file exists and
        ///   <paramref name="append" /> is false, the file is overwritten. If the file exists and <paramref name="append" /> is
        ///   true, the data is appended to the file. Otherwise, a new file is created.
        /// </param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="bufferSize">Sets the buffer size.</param>
        /// <param name="formatProvider">IFormatProvider to use.</param>
        public FormattingStreamWriter([NotNull] string path, bool append, [NotNull] Encoding encoding, int bufferSize,
            [NotNull] IFormatProvider formatProvider)
            : base(path, append, encoding, bufferSize)
        {
            if (formatProvider == null) throw new ArgumentNullException("formatProvider");
            _formatProvider = formatProvider;
        }


        /// <summary>
        ///   Initializes a new instance of the <see cref="FormattingStreamWriter" /> class.
        /// </summary>
        /// <param name="path">The complete file path to write to.</param>
        /// <param name="append">
        ///   Determines whether data is to be appended to the file. If the file exists and
        ///   <paramref name="append" /> is false, the file is overwritten. If the file exists and <paramref name="append" /> is
        ///   true, the data is appended to the file. Otherwise, a new file is created.
        /// </param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="formatProvider">IFormatProvider to use.</param>
        public FormattingStreamWriter([NotNull] string path, bool append, [NotNull] Encoding encoding,
            [NotNull] IFormatProvider formatProvider)
            : base(path, append, encoding)
        {
            if (formatProvider == null) throw new ArgumentNullException("formatProvider");
            _formatProvider = formatProvider;
        }


        /// <summary>
        ///   Initializes a new instance of the <see cref="FormattingStreamWriter" /> class.
        /// </summary>
        /// <param name="path">The complete file path to write to.</param>
        /// <param name="append">
        ///   Determines whether data is to be appended to the file. If the file exists and
        ///   <paramref name="append" /> is false, the file is overwritten. If the file exists and <paramref name="append" /> is
        ///   true, the data is appended to the file. Otherwise, a new file is created.
        /// </param>
        /// <param name="formatProvider">IFormatProvider to use.</param>
        public FormattingStreamWriter([NotNull] string path, bool append, [NotNull] IFormatProvider formatProvider)
            : base(path, append)
        {
            if (formatProvider == null) throw new ArgumentNullException("formatProvider");
            _formatProvider = formatProvider;
        }


        /// <summary>
        ///   Initializes a new instance of the <see cref="FormattingStreamWriter" /> class.
        /// </summary>
        /// <param name="path">The complete file path to write to. <paramref name="path" /> can be a file name.</param>
        /// <param name="formatProvider">IFormatProvider to use.</param>
        public FormattingStreamWriter([NotNull] string path, [NotNull] IFormatProvider formatProvider)
            : base(path)
        {
            if (formatProvider == null) throw new ArgumentNullException("formatProvider");
            _formatProvider = formatProvider;
        }


        /// <summary>
        ///   Initializes a new instance of the <see cref="FormattingStreamWriter" /> class.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="bufferSize">Sets the buffer size.</param>
        /// <param name="formatProvider">IFormatProvider to use.</param>
        public FormattingStreamWriter([NotNull] Stream stream, [NotNull] Encoding encoding, int bufferSize,
            [NotNull] IFormatProvider formatProvider)
            : base(stream, encoding, bufferSize)
        {
            if (formatProvider == null) throw new ArgumentNullException("formatProvider");
            _formatProvider = formatProvider;
        }


        /// <summary>
        ///   Initializes a new instance of the <see cref="FormattingStreamWriter" /> class.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="formatProvider">IFormatProvider to use.</param>
        public FormattingStreamWriter([NotNull] Stream stream, [NotNull] Encoding encoding,
            [NotNull] IFormatProvider formatProvider)
            : base(stream, encoding)
        {
            if (formatProvider == null) throw new ArgumentNullException("formatProvider");
            _formatProvider = formatProvider;
        }


        /// <summary>
        ///   Initializes a new instance of the <see cref="FormattingStreamWriter" /> class.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="formatProvider">IFormatProvider to use.</param>
        public FormattingStreamWriter([NotNull] Stream stream, [NotNull] IFormatProvider formatProvider)
            : base(stream)
        {
            if (formatProvider == null) throw new ArgumentNullException("formatProvider");
            _formatProvider = formatProvider;
        }


        /// <summary>
        ///   Gets and sets an object that controls formatting.
        /// </summary>
        public override IFormatProvider FormatProvider
        {
            get { return _formatProvider; }
        }
    }
}