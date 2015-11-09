#region copyright

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

namespace Alphacloud.Common.Core.Threading
{
#if NET45 || NET46
    using System;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    /// <summary>
    ///   Async tasks helper.
    /// </summary>
    [PublicAPI]
    public static class AsyncTask
    {
        /// <summary>
        ///   Creates completed void task.
        /// </summary>
        /// <returns></returns>
        public static Task Completed()
        {
            return Task.FromResult(0);
        }


        /// <summary>
        ///   Wraps asynchronous task in synchronous wrapper.
        ///   Method will block unit tasks completes.
        /// </summary>
        /// <typeparam name="TResult">Result</typeparam>
        /// <param name="asyncTask">Asynchronous task</param>
        /// <param name="continueOnCapturedContext"></param>
        /// <returns>Execution result.</returns>
        public static TResult Synchronize<TResult>([NotNull] Func<Task<TResult>> asyncTask,
            bool continueOnCapturedContext = false)
        {
            if (asyncTask == null) throw new ArgumentNullException(nameof(asyncTask));

            return
                Task.Run(async () => await asyncTask().ConfigureAwait(continueOnCapturedContext))
                    .GetAwaiter()
                    .GetResult();
        }
    }

#endif
}