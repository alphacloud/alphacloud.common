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

namespace Alphacloud.Common.Testing.Nunit
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Data;
    using Moq;

    /// <summary>
    ///   Custom argument matchers
    /// </summary>
    public static class Argument
    {
        /// <summary>
        ///   Collection matcher.
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="expectation">Expected collection.</param>
        /// <returns>Matcher.</returns>
        public static IEnumerable<T> IsCollection<T>(IEnumerable<T> expectation)
        {
            return Match.Create<IEnumerable<T>>(inputCollection => expectation.All(inputCollection.Contains));
        }


        /// <summary>
        ///   Collection matcher.
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="expected">Expected collection.</param>
        /// <returns>Matcher.</returns>
        public static ICollection<T> IsCollection<T>(ICollection<T> expected)
        {
            return Match.Create<ICollection<T>>(inputCollection => expected.All(inputCollection.Contains));
        }


        public static IDictionary<TKey, TValue> IsDictionary<TKey, TValue>(IDictionary<TKey, TValue> expected)
        {
            return Match.Create<IDictionary<TKey, TValue>>(actual => SameDictionary(actual, expected));
        }


        static bool SameDictionary<TKey, TValue>(IDictionary<TKey, TValue> actual, IDictionary<TKey, TValue> expected)
        {
            return expected.All(kvp => Equals(kvp.Value, actual.ValueOrDefault(kvp.Key)));
        }
    }
}