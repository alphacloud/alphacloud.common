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
    using System.Threading;

    using JetBrains.Annotations;

    #endregion

    /// <summary>
    /// <see cref="ReaderWriterLockSlim"/> extensions.
    /// </summary>
    [PublicAPI]
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
        public static T Read <T>([NotNull] this ReaderWriterLockSlim locker, [NotNull] Func<T> read)
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
