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
        public static TValue ValueOrDefault<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dictionary, TKey key)
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
        public static TValue ValueOrDefault<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dictionary, TKey key,
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
        public static TValue GetOrRetrieve<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dictionary, TKey key,
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
    }
}