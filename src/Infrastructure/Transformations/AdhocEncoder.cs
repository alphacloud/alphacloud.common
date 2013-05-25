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

namespace Alphacloud.Common.Infrastructure.Transformations
{

    using System;


    /// <summary>
    ///   Implements converter that invokes specific delegate for conversion.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AdHocEncoder <T> : IEncoder<T>
    {
        static readonly IEncoder<T> s_null = new AdHocEncoder<T>(t => t);

        readonly Func<T, T> _encode;

        /// <summary>
        ///   Initializes a new instance of the <see cref="AdHocEncoder{T}" /> class.
        /// </summary>
        /// <param name="encode">The delegate to invoke for conversion.</param>
        public AdHocEncoder(Func<T, T> encode)
        {
            if (encode == null)
            {
                throw new ArgumentNullException("encode");
            }
            _encode = encode;
        }


        /// <summary>
        ///   Null object.
        /// </summary>
        public static IEncoder<T> Null
        {
            get { return s_null; }
        }

        /// <summary>
        ///   Converts specified source value.
        /// </summary>
        /// <param name="source">The value.</param>
        /// <returns>Converted value.</returns>
        public T Encode(T source)
        {
            return _encode(source);
        }
    }
}
