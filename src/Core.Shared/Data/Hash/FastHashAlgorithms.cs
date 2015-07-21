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

namespace Alphacloud.Common.Core.Data.Hash
{
    using JetBrains.Annotations;


    /// <summary>
    ///   Fast non-cryptographic algorithms.
    /// </summary>
    [PublicAPI]
    public static class FastHashAlgorithms
    {
        static readonly ISimpleHasher s_murmur3 = new Murmur3();

        /// <summary>
        ///   Murmur3 hash.
        /// </summary>
        public static ISimpleHasher Murmur3
        {
            get { return s_murmur3; }
        }


        /// <summary>
        ///   Computes hash of byte array.
        /// </summary>
        /// <param name="hash">The hash algorithm.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        ///   <c>0</c> in case data is <c>null</c> or empty array; hash otherwise.
        /// </returns>
        public static uint Compute([NotNull] this ISimpleHasher hash, [CanBeNull] byte[] data)
        {
            if (data == null || data.Length == 0)
                return 0;
            return hash.Compute(data, 0, data.Length);
        }
    }
}