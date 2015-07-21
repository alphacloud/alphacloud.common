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

namespace Alphacloud.Common.Core.Utils
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;

    using JetBrains.Annotations;

    #endregion

    /// <summary>
    ///   Disposes registered objects.
    /// 
    /// Used to manually dispose set of objects.
    /// Registered objects will be disposed then Disposer is disposed.
    /// </summary>
    /// <remarks>
    /// Registered objects are stored as <see cref="WeakReference"/> to they could be processed by garbadge collector earlier.
    /// </remarks>
    [PublicAPI]
    public sealed class Disposer : IDisposable
    {
        readonly IList<WeakReference> _objects = new List<WeakReference>();
        bool _isDisposed;


        /// <summary>
        /// Initializes a new instance of the <see cref="Disposer"/> class.
        /// </summary>
        public Disposer()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Disposer"/> class.
        /// </summary>
        /// <param name="disposable">The disposable.</param>
        /// <exception cref="System.ArgumentNullException">disposable</exception>
        public Disposer([NotNull] IDisposable disposable)
        {
            if (disposable == null)
                throw new ArgumentNullException("disposable");
            Add(disposable);
        }


        /// <summary>
        /// Disposes object if it is not null and implements IDisposable interface.
        /// </summary>
        /// <param name="obj">Object to dispose.</param>
        /// <returns><c>true</c>if object was disposed.</returns>
        public static bool TryDispose([CanBeNull] object obj)
        {
            var disposable = obj as IDisposable;
            if (disposable == null)
                return false;
            disposable.Dispose();
            return true;
        }

        /// <summary>
        ///   Register object to be disposed.
        /// </summary>
        /// <param name="obj">Object</param>
        [NotNull]
        public T Add <T>([NotNull] T obj)
            where T : IDisposable
        {
            CheckDisposed();
// ReSharper disable CompareNonConstrainedGenericWithNull
            if (obj == null)
                throw new ArgumentNullException("obj");
// ReSharper restore CompareNonConstrainedGenericWithNull

            _objects.Add(new WeakReference(obj));
            return obj;
        }


        /// <summary>
        /// Register object to be disposed.
        /// </summary>
        /// <param name="obj">Object to dispose.</param>
        public void Add(IDisposable obj)
        {
            CheckDisposed();
            if (obj == null)
                return;
            _objects.Add(new WeakReference(obj));
        }


        /// <summary>
        ///   Check if this object was disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">If object already disposed.</exception>
        void CheckDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException("Disposer");
        }

        #region Implementation of IDisposable

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            // dispose in reverse order
            for (var i = _objects.Count - 1; i >= 0; i--)
            {
                var reference = _objects[i];
                if (!reference.IsAlive)
                    continue;
                TryDispose(reference.Target as IDisposable);
            }
            _isDisposed = true;
        }

        #endregion
    }
}
