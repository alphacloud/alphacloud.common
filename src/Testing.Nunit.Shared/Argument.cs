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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Data;
    using JetBrains.Annotations;
    using Moq;

    /// <summary>
    ///   Custom argument matchers
    /// </summary>
    [PublicAPI]
    public static class Argument
    {
        /// <summary>
        ///   Collection matcher.
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="expected">Expected collection.</param>
        /// <returns>Matcher.</returns>
        public static IEnumerable<T> IsCollection<T>(IEnumerable<T> expected)
        {
            return Match.Create<IEnumerable<T>>(inputCollection => expected.All(inputCollection.Contains));
        }


        /// <summary>
        ///   Collection matcher.
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="expected">Expected collection.</param>
        /// <returns>Matcher.</returns>
        public static ICollection<T> IsCollection<T>([NotNull] ICollection<T> expected)
        {
            if (expected == null) throw new ArgumentNullException("expected");
            return Match.Create<ICollection<T>>(inputCollection => expected.All(inputCollection.Contains));
        }

        /// <summary>
        ///   Array matcher.
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="expected">Expected collection.</param>
        /// <returns>Matcher.</returns>
        public static T[] IsArray<T>([NotNull] IEnumerable<T> expected)
        {
            if (expected == null) throw new ArgumentNullException("expected");
            return Match.Create<T[]>(inputCollection => expected.All(inputCollection.Contains));
        }


        /// <summary>
        ///   Dictionary matcher.
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue ">Value type</typeparam>
        /// <param name="expected">Expected dictionary.</param>
        /// <returns>Matcher.</returns>
        public static Dictionary<TKey, TValue> IsDictionary<TKey, TValue>(IDictionary<TKey, TValue> expected)
        {
            return Match.Create<Dictionary<TKey, TValue>>(actual => SameDictionary(actual, expected));
        }


        static bool SameDictionary<TKey, TValue>(IDictionary<TKey, TValue> actual, IDictionary<TKey, TValue> expected)
        {
            return expected.All(kvp => Equals(kvp.Value, actual.ValueOrDefault(kvp.Key)));
        }
    }
}