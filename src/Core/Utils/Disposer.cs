namespace Alphacloud.Common.Core.Utils
{
    #region using

    using System;
    using System.Collections.Generic;

    using JetBrains.Annotations;

    #endregion

    /// <summary>
    ///     Dispose helper.
    /// </summary>
    [PublicAPI]
    public class Disposer : IDisposable
    {
        readonly IList<WeakReference> _objects = new List<WeakReference>();
        bool _isDisposed;


        public Disposer()
        {
        }


        public Disposer([NotNull] IDisposable disposable)
        {
            if (disposable == null)
                throw new ArgumentNullException("disposable");
            Add(disposable);
        }


        /// <summary>
        ///     Add object to dispose.
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


        public void Add(IDisposable obj)
        {
            CheckDisposed();
            if (obj == null)
                return;
            _objects.Add(new WeakReference(obj));
        }


        /// <summary>
        ///     Check if object was disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">If object already disposed.</exception>
        protected void CheckDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException("Disposer");
        }

        #region Implementation of IDisposable

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;
            if (disposing)
            {
                // dispose in reverse order
                for (var i = _objects.Count - 1; i >= 0; i--)
                {
                    var reference = _objects[i];
                    if (!reference.IsAlive)
                        continue;
                    var obj = reference.Target as IDisposable;
                    if (obj != null)
                        obj.Dispose();
                }
                _isDisposed = true;
            }
        }

        #endregion
    }
}
