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

namespace Alphacloud.Common.Infrastructure.Caching
{
    using System;
    using Core.Data;
    using JetBrains.Annotations;
    using Transformations;

    /// <summary>
    ///   Implements cache wrapper that transforms keys or values
    ///   during their way to/from wrapped cache instance.
    /// </summary>
    [PublicAPI]
    public class TransformingCacheWrapper : CacheBase
    {
        private readonly ICache _innerCache;
        private readonly IEncoder<string> _keyTransformer;
        private readonly ITransformer<object> _valueTransformer;


        /// <summary>
        ///   Initializes a new instance of the <see cref="TransformingCacheWrapper" /> class.
        /// </summary>
        /// <param name="innerCache">The inner cache.</param>
        /// <param name="keyTransformer">The key transformer.</param>
        /// <param name="name">Instance name</param>
        /// <param name="valueTransformer">The value transformer.</param>
        public TransformingCacheWrapper(ICache innerCache,
            string name,
            IEncoder<string> keyTransformer = null,
            ITransformer<object> valueTransformer = null) : base(NullCacheHealthcheckMonitor.Instance, name)
        {
            if (innerCache == null)
            {
                throw new ArgumentNullException("innerCache");
            }

            _innerCache = innerCache;
            _keyTransformer = keyTransformer ?? AdHocEncoder<string>.Null;
            _valueTransformer = valueTransformer ?? AdHocTransformer<object>.Null;
        }


        /// <summary>
        ///   Gets cache instance name.
        /// </summary>
        /// <value>
        ///   The name.
        /// </value>
        public override string Name
        {
            get { return "{0}.{1}".ApplyArgs(base.Name, _innerCache.Name); }
        }


        protected internal override CacheStatistics DoGetStatistics()
        {
            throw new InvalidOperationException("Should override GetStatistics()");
        }


        /// <summary>
        ///   Gets data from cache by key.
        /// </summary>
        /// <param name="key">The key for the data.</param>
        /// <returns>Cached value or null if it expired or does not exist.</returns>
        public override object Get(string key)
        {
            var encodedKey = _keyTransformer.Encode(key);
            var encodedValue = _innerCache.Get(encodedKey);
            var value = _valueTransformer.Decode(encodedValue);

            return value;
        }


        /// <summary>
        ///   Get cache statistics
        /// </summary>
        /// <returns>
        ///   Cache statistics
        /// </returns>
        public override CacheStatistics GetStatistics()
        {
            return _innerCache.GetStatistics();
        }


        /// <summary>
        ///   Clears the entire cache.
        /// </summary>
        public override void Clear()
        {
            _innerCache.Clear();
        }


        public override void Remove(string key)
        {
            var encodedKey = _keyTransformer.Encode(key);
            _innerCache.Remove(encodedKey);
        }


        /// <summary>
        ///   Add item to cache.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Item</param>
        /// <param name="ttl">Absolute expiration timeout.</param>
        /// <remarks>
        ///   If <paramref name="value" /> is <c>null</c>, <see cref="Remove" /> item will be removed from cache by calling
        ///   <see
        ///     cref="Remove" />
        ///   .
        /// </remarks>
        public override void Put(string key, object value, TimeSpan ttl)
        {
            var encodedkey = _keyTransformer.Encode(key);
            var encodedValue = _valueTransformer.Encode(value);

            _innerCache.Put(encodedkey, encodedValue, ttl);
        }


        /// <summary>
        ///   Invokes Get operation on underlying cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   Value or <c>null</c>
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Get() should call inner cache.</exception>
        protected internal override object DoGet(string key)
        {
            throw new InvalidOperationException("Get() should call inner cache.");
        }


        protected internal override void DoClear()
        {
            throw new InvalidOperationException("Clear() should call inner cache.");
        }


        protected internal override void DoRemove(string key)
        {
            throw new InvalidOperationException("Remove() should call inner cache.");
        }


        protected internal override void DoPut(string key, object value, TimeSpan ttl)
        {
            throw new InvalidOperationException("Put() should call inner cache.");
        }
    }
}