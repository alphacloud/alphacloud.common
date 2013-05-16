namespace Alphacloud.Common.Core.Utils
{
    #region using

    using System;
    using System.Threading;
    using JetBrains.Annotations;

    #endregion

    public static class ReaderWriterLockSlimExtensions
    {
        /// <summary>
        ///   Excecute read operation.
        /// </summary>
        /// <param name="locker"></param>
        /// <param name="action">operation</param>
        public static void Read([NotNull] this ReaderWriterLockSlim locker, [NotNull] Action action)
        {
            if (locker == null)
                throw new ArgumentNullException("locker");
            if (action == null)
                throw new ArgumentNullException("action");

            locker.EnterReadLock();
            try
            {
                action();
            }
            finally
            {
                locker.ExitReadLock();
            }
        }


        /// <summary>
        ///   Excecute read operation.
        /// </summary>
        /// <param name="locker"></param>
        /// <param name="read">operation</param>
        public static T Read<T>([NotNull] this ReaderWriterLockSlim locker, [NotNull] Func<T> read)
        {
            if (locker == null)
                throw new ArgumentNullException("locker");
            if (read == null)
                throw new ArgumentNullException("read");

            locker.EnterReadLock();
            try
            {
                return read();
            }
            finally
            {
                locker.ExitReadLock();
            }
        }


        /// <summary>
        ///   Execute write operation.
        /// </summary>
        /// <param name="locker"></param>
        /// <param name="action">operation</param>
        public static void Write([NotNull] this ReaderWriterLockSlim locker, [NotNull] Action action)
        {
            if (locker == null)
                throw new ArgumentNullException("locker");
            if (action == null)
                throw new ArgumentNullException("action");

            locker.EnterWriteLock();
            try
            {
                action();
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }
    }
}