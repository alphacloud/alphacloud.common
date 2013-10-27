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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using JetBrains.Annotations;

    /// <summary>
    ///   Collection extenstion methods.
    /// </summary>
    [PublicAPI]
    public static class CollectionExtensions
    {
        /// <summary>
        ///   Remove first occurence of item matching predicate.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="collection"> The collection. </param>
        /// <param name="predicate"> The predicate. </param>
        /// <returns> </returns>
        public static bool RemoveFirst <T>([NotNull] this ICollection<T> collection, [NotNull] Func<T, bool> predicate)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            var item = collection.FirstOrDefault(predicate);
            if (Equals(item, default(T)))
                return false;

            collection.Remove(item);
            return true;
        }


        /// <summary>
        ///   Repeat action for all items in <paramref name="sequence" />
        /// </summary>
        /// <typeparam name="T"> Type </typeparam>
        /// <param name="sequence"> Source sequence. </param>
        /// <param name="process"> Renderer. </param>
        public static void Repeat <T>(this IEnumerable<T> sequence, Action<CurrentItem<T>> process)
        {
            if (sequence == null)
                return;

            var list = sequence.ToList();
            var loop = new CurrentItem<T>(list);

            var i = 0;
            list.ForEach(item => process(loop.Update(i++, item)));
        }


        /// <summary>
        ///   Repeat action for all items in <paramref name="collection" />
        /// </summary>
        /// <typeparam name="T"> Type </typeparam>
        /// <param name="collection"> Source sequence. </param>
        /// <param name="process"> Renderer. </param>
        public static void Repeat <T>(ICollection<T> collection, Action<T, CurrentItem<T>> process)
        {
            if (collection == null)
                return;

            var loop = new CurrentItem<T>(collection);

            var i = 0;
            foreach (var item in collection)
                process(item, loop.Update(i++, item));
        }


        /// <summary>
        ///   Returns zero-based index the first matching item in sequence.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   Zero-based index or <c>-1</c> if item was not found.
        /// </returns>
        public static int IndexOf <T>([NotNull] this IEnumerable<T> sequence, T item)
        {
            if (sequence == null)
                throw new ArgumentNullException("sequence");

            var index = 0;
            foreach (var currentItem in sequence)
            {
                if (Equals(currentItem, item))
                    return index;
                index++;
            }
            return -1;
        }

        /// <summary>
        ///   Returns zero-based index the first matching item in sequence.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="predicate">The search predicate.</param>
        /// <returns>
        ///   Zero-based index or <c>-1</c> if item was not found.
        /// </returns>
        public static int IndexOf <T>([NotNull] this IEnumerable<T> sequence, [NotNull] Func<T, bool> predicate)
        {
            if (sequence == null)
                throw new ArgumentNullException("sequence");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            var index = 0;
            foreach (var currentItem in sequence)
            {
                if (predicate(currentItem))
                    return index;
                index++;
            }
            return -1;
        }


        /// <summary>
        ///   Execute action on all sequence items.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="sequence"> The source. </param>
        /// <param name="action"> The action. </param>
        /// <returns> Number of items processed. </returns>
        public static int ForAll <T>([NotNull] this IEnumerable<T> sequence, [NotNull] Action<T> action)
        {
            if (sequence == null)
                throw new ArgumentNullException("sequence");
            if (action == null)
                throw new ArgumentNullException("action");
            int processed = 0;
            foreach (var item in sequence)
            {
                action(item);
                processed++;
            }
            return processed;
        }


        /// <summary>
        ///   Execute action on all sequence items.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="sequence"> The source. </param>
        /// <param name="action"> The action to run (first param - item, 2nd param - item index). </param>
        /// <returns> Number of items processed. </returns>
        public static int ForAll <T>([NotNull] this IEnumerable<T> sequence, [NotNull] Action<T, int> action)
        {
            if (sequence == null)
                throw new ArgumentNullException("sequence");
            if (action == null)
                throw new ArgumentNullException("action");
            int processed = 0;
            foreach (var item in sequence)
            {
                action(item, processed);
                processed++;
            }
            return processed;
        }


        /// <summary>
        ///   Takes page of data from sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <exception cref="ArgumentException">Page number &lt;= 0 or Page size &lt;= 0. </exception>
        /// <returns></returns>
        public static IEnumerable<T> TakePage <T>([NotNull] this IEnumerable<T> source, int pageNumber, int pageSize)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (pageNumber < 1)
                throw new ArgumentException("Page number must be greater than zero.", "pageNumber");
            if (pageSize < 1)
                throw new ArgumentException("Page size must be greater than zero.", "pageSize");
            return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }


        /// <summary>
        ///   Try get values from lookup.
        /// </summary>
        /// <typeparam name="TKey"> The type of the key. </typeparam>
        /// <typeparam name="TElement"> The type of the element. </typeparam>
        /// <param name="lookup"> The lookup. </param>
        /// <param name="key"> The key. </param>
        /// <returns> Values or empty sequence if key not found. </returns>
        public static IEnumerable<TElement> TryGetValues <TKey, TElement>(this ILookup<TKey, TElement> lookup, TKey key)
        {
            if (lookup == null || !lookup.Contains(key))
                return Enumerable.Empty<TElement>();

            return lookup[key];
        }

        /// <summary>
        ///     Determines whether collection is <c>null</c> or does not contain items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>
        ///     <c>true</c> if collection is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>([CanBeNull] this ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }

    }


    /// <summary>
    ///   Represents sequence of single item.
    /// </summary>
    /// <typeparam name="T">Type of item.</typeparam>
    public class SingleItemSequence <T> : IEnumerable<T>
    {
        readonly T _item;

        #region .ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleItemSequence{T}"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public SingleItemSequence(T item)
        {
            _item = item;
        }

        #endregion

        #region Implementation of IEnumerable

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///   A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<T> GetEnumerator()
        {
            yield return _item;
        }


        /// <summary>
        ///   Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///   An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }


    /// <summary>
    ///   Provides intormation about current item in the repeater.
    /// </summary>
    /// <typeparam name="T"> Type </typeparam>
    [PublicAPI]
    public class CurrentItem <T>
    {
        readonly int _itemCount;

        #region .ctor

        /// <summary>
        ///   Initializes a new instance of the <see cref="CurrentItem{T}" /> class.
        /// </summary>
        /// <param name="items">Items to operate on.</param>
        protected internal CurrentItem(ICollection<T> items)
        {
            _itemCount = items.Count;
        }

        #endregion

        /// <summary>
        ///   Current item index (1-based).
        /// </summary>
        public int Counter { get; private set; }

        /// <summary>
        ///   Current item index (zero-based).
        /// </summary>
        public int Counter0 { get; private set; }

        /// <summary>
        ///   Check whether current item is first in sequence.
        /// </summary>
        /// <value>
        ///   <c>true</c> if current item is first; otherwise, <c>false</c> .
        /// </value>
        public bool IsFirst { get; private set; }

        /// <summary>
        ///   Check whether current item is last in sequence.
        /// </summary>
        /// <value>
        ///   <c>true</c> if current item is last; otherwise, <c>false</c> .
        /// </value>
        public bool IsLast { get; private set; }

        /// <summary>
        ///   Current item of the repeater.
        /// </summary>
        public T Item { get; private set; }

        /// <summary>
        ///   Update current item.
        /// </summary>
        /// <param name="index">0-based item index.</param>
        /// <param name="item">Item</param>
        /// <returns>Self.</returns>
        protected internal CurrentItem<T> Update(int index, T item)
        {
            Counter = index + 1;
            Counter0 = index;
            IsFirst = index == 0;
            IsLast = index == _itemCount - 1;
            Item = item;
            return this;
        }
    }
}
