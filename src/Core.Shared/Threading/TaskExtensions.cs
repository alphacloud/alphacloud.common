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

namespace Alphacloud.Common.Core.Threading
{
    #region using

    using System;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    #endregion

    /// <summary>
    /// TPL helpers.
    /// </summary>
    [PublicAPI]
    public static class TaskExtensions
    {
        /// <summary>
        ///   Wait for task completion ignoring OperationCancelled exceptions.
        /// </summary>
        /// <param name="task">The task to wait for.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        public static bool SafeWait(this Task task, TimeSpan timeout)
        {
            try
            {
                return task.Wait(timeout);
            }
            catch (AggregateException ex)
            {
                ex.Handle(e => e is OperationCanceledException);
                return true;
            }
        }
    }
}
