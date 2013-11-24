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

using System.Collections.Generic;
using System.Linq;
using Moq;

namespace Alphacloud.Common.Testing.Nunit
{
    /// <summary>
    /// Custom argument matchers
    /// </summary>
    public static class Argument
    {
        /// <summary>
        /// Collection matcher.
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="expectation">Expected collection.</param>
        /// <returns>Matcher.</returns>
        public static IEnumerable<T> IsCollection<T>(IEnumerable<T> expectation)
        {
            return Match.Create<IEnumerable<T>>(inputCollection => expectation.All(inputCollection.Contains));
        }


        /// <summary>
        /// Collection matcher.
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="expectation">Expected collection.</param>
        /// <returns>Matcher.</returns>
        public static ICollection<T> IsCollection<T>(ICollection<T> expectation)
        {
            return Match.Create<ICollection<T>>(inputCollection => expectation.All(inputCollection.Contains));
        }

    }
}