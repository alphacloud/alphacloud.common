﻿#region copyright

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

namespace Alphacloud.Common.Core.Utils
{
    using System;
    using System.Collections.Concurrent;
    using Data;
    using JetBrains.Annotations;

    /// <summary>
    ///   Naive object pool implementation.
    /// </summary>
    /// <remarks>
    ///   Holds pool of given maximum size.
    ///   In case of empty pool, new objects will be created using provided factory method.
    /// </remarks>
    [PublicAPI]
    public class ObjectPool<T> : IObjectPool<T> where T : class
    {
        readonly int _maxPoolSize;
        readonly Func<T> _objectConstructor;
        readonly ConcurrentBag<T> _pool;


        public ObjectPool(int maxPoolSize, [NotNull] Func<T> objectConstructor)
        {
            if (objectConstructor == null) throw new ArgumentNullException("objectConstructor");
            if (maxPoolSize <= 2 || maxPoolSize > 10000)
                throw new ArgumentOutOfRangeException("maxPoolSize", maxPoolSize,
                    @"Object pool size must be between 2 and 10000.");

            _maxPoolSize = maxPoolSize;
            _objectConstructor = objectConstructor;
            _pool = new ConcurrentBag<T>();
        }

        #region IObjectPool<T> Members

        /// <summary>
        /// Returns number of objects in the pool.
        /// </summary>
        public int Count => _pool.Count;


        /// <summary>
        ///   Retrieve object from pool or create new if pool is empty.
        /// </summary>
        /// <returns>Object requested.</returns>
        public T GetObject()
        {
            T result;
            if (!_pool.TryTake(out result))
            {
                result = _objectConstructor();
            }
            return result;
        }


        /// <summary>
        ///   Retrieve or create object and wrapper it with disposable <see cref="PooledObjectWrapper{T}" />.
        /// </summary>
        /// <returns>Wrapped object.</returns>
        public PooledObjectWrapper<T> GetWrappedObject()
        {
            return new PooledObjectWrapper<T>(this, GetObject());
        }


        /// <summary>
        ///   Return object back to pool.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <remarks>
        ///   In case pool is already reached maximum size, object will be disposed.
        /// </remarks>
        public void ReturnObject([NotNull] T obj)
        {
            if (ShouldStore(obj))
            {
                _pool.Add(obj);
            }
            else
            {
                Disposer.TryDispose(obj);
            }
        }

        #endregion

        /// <summary>
        /// Determines if object should be stored in pool.
        /// </summary>
        /// <returns><c>true</c> - object should be stored in pool, <c>false</c> - object should be discarded.</returns>
        protected virtual bool ShouldStore([CanBeNull] T obj)
        {
            return (obj != null) && _pool.Count < _maxPoolSize;
        }
    }

    /// <summary>
    ///   Naive object pool.
    /// </summary>
    /// <typeparam name="T">Type of pooled objects.</typeparam>
    /// <remarks>
    ///   All implementations must be thread-safe.
    /// </remarks>
    public interface IObjectPool<T>
        where T : class
    {
        /// <summary>
        ///   Gets current pool size.
        /// </summary>
        /// <value>
        ///   Pool size.
        /// </value>
        int Count { get; }


        /// <summary>
        ///   Retrieve object from pool or create new if pool is empty.
        /// </summary>
        /// <returns>Object requested.</returns>
        T GetObject();


        /// <summary>
        ///   Retrieve or create object and wrapper it with disposable <see cref="PooledObjectWrapper{T}" />.
        /// </summary>
        /// <returns>Wrapped object.</returns>
        PooledObjectWrapper<T> GetWrappedObject();


        /// <summary>
        ///   Return object back to pool.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <remarks>
        ///   In case pool is already reached maximum size, object will be disposed.
        /// </remarks>
        void ReturnObject([NotNull] T obj);
    }

    /// <summary>
    ///   Returns object back to pool on disposal.
    /// </summary>
    /// <typeparam name="T">Type of object.</typeparam>
    /// <remarks>
    ///   Holds weak reference to parent pool.
    /// </remarks>
    public sealed class PooledObjectWrapper<T> : IDisposable
        where T : class
    {
        readonly WeakReference _owner;
        bool _isDisposed;
        T _value;


        /// <summary>
        ///   Initializes a new instance of the <see cref="PooledObjectWrapper{T}" /> class.
        /// </summary>
        /// <param name="owner">The owning pool.</param>
        /// <param name="obj">The object.</param>
        /// <exception cref="System.ArgumentNullException">
        ///   owner or obj is null.
        /// </exception>
        public PooledObjectWrapper([NotNull] IObjectPool<T> owner, [NotNull] T obj)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            _value = obj;
            _owner = new WeakReference(owner);
        }


        /// <summary>
        ///   Returns pooled object.
        /// </summary>
        /// <value>
        ///   The Object.
        /// </value>
        /// <exception cref="System.ObjectDisposedException">Pooled object already disposed.</exception>
        public T Value
        {
            get
            {
                if (_isDisposed)
                    throw new ObjectDisposedException("Pooled object already disposed and returned to pool.");
                return _value;
            }
        }

        #region IDisposable Members

        /// <summary>
        ///   Returns assosicated object to pool.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            var pool = _owner.Target as IObjectPool<T>;
            if (pool != null)
            {
                pool.ReturnObject(_value);
            }
            _value = null;
        }

        #endregion
    }

    /// <summary>
    ///   Object serializer pool.
    /// </summary>
    /// <remarks>
    ///   This pools won't store object if allocated memory stream size exceeds specified.
    /// </remarks>
    public sealed class SerializerPool : ObjectPool<ISerializer>
    {
        readonly int _maxPooledMemoryBlockToStore;


        public SerializerPool(int maxPoolSize, [NotNull] Func<ISerializer> objectConstructor,
            int maxPooledMemoryBlockSize)
            : base(maxPoolSize, objectConstructor)
        {
            if (maxPooledMemoryBlockSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxPooledMemoryBlockSize), maxPooledMemoryBlockSize,
                    @"Memory block size must ebe positive number.");
            _maxPooledMemoryBlockToStore = maxPooledMemoryBlockSize;
        }


        /// <summary>
        /// Determines if object should be stored in pool.
        /// </summary>
        protected override bool ShouldStore(ISerializer obj)
        {
            return base.ShouldStore(obj) && obj.AllocatedMemorySize <= _maxPooledMemoryBlockToStore;
        }
    }
}