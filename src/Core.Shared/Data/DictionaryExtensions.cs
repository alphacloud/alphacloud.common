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

namespace Alphacloud.Common.Core.Data
{
    #region using

    using System;
    using System.Collections.Generic;

    using JetBrains.Annotations;

    #endregion

    /// <summary>
    ///   Dictionary extension methods.
    /// </summary>
    [PublicAPI]
    public static class DictionaryExtensions
    {
        /// <summary>
        ///   Return existing or default value from dictionary.
        /// </summary>
        /// <remarks>
        ///   This method simulates Hashtable behaviour.
        /// </remarks>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <returns>Return value from dictionary or default value if key not exists.</returns>
        public static TValue ValueOrDefault <TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.ValueOrDefault(key, default(TValue));
        }


        /// <summary>
        ///   Return existing or default value from dictionary.
        /// </summary>
        /// <remarks>
        ///   This method simulates Hashtable behaviour.
        /// </remarks>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">Value to return for non-existing key.</param>
        /// <returns>Return value from dictionary or specified default value if key not exists.</returns>
        public static TValue ValueOrDefault <TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dictionary, TKey key,
            TValue defaultValue)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");
            TValue val;
            return dictionary.TryGetValue(key, out val) ? val : defaultValue;
        }

        /// <summary>
        ///   Get existing or retrieve new and add to dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="retrieve">The retrieve function.</param>
        /// <returns></returns>
        public static TValue GetOrRetrieve <TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dictionary, TKey key,
            [NotNull] Func<TKey, TValue> retrieve)
        {
            if (dictionary == null) throw new ArgumentNullException("dictionary");
            if (retrieve == null) throw new ArgumentNullException("retrieve");

            TValue val;
            if (dictionary.TryGetValue(key, out val))
                return val;

            val = retrieve(key);
            dictionary.Add(key, val);
            return val;
        }
        /// <summary>
        /// Add item if it does not exists in dictionary.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns><c>true</c>if item was added; <c>false</c> if item was exist in dictionary.</returns>
        public static bool AddNew<K, V>([NotNull] this IDictionary<K, V> dictionary, K key, V value)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }
            bool isNew = !dictionary.ContainsKey(key);
            if (isNew)
                dictionary[key] = value;

            return isNew;
        }
    }
}
