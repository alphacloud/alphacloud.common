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

namespace Alphacloud.Common.Core.Data.Hash
{
    using System.Collections.Generic;
    using JetBrains.Annotations;

    /// <summary>
    ///   generic fast hash algorithm.
    /// </summary>
    public interface ISimpleHasher
    {
        /// <summary>
        ///   Computes the hash for specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="start">The start position.</param>
        /// <param name="size">The size.</param>
        /// <returns>32bit hash</returns>
        uint Compute([NotNull] byte[] array, int start, int size);


        /// <summary>
        ///   Computes the hash for specified array.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>32bit hash</returns>
        uint Compute([NotNull] int[] data);


        /// <summary>
        ///   Computes the hash for specified data.
        ///   Use this overload to avoid calling.ToArray()
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>32bit hash</returns>
        uint Compute([NotNull] IList<int> data);
    }
}