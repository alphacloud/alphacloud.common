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

namespace Alphacloud.Common.Infrastructure.Instrumentation
{
    using System;
    using Core.Instrumentation;

    /// <summary>
    ///   Contains instrumentation event data.
    /// </summary>
    public class InstrumentationEventArgs : EventArgs
    {
        /// <summary>
        ///   Correlation id.
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        ///   Id of thread which which was executing operation.
        /// </summary>
        public int ManagedThreadId { get; set; }

        /// <summary>
        ///   Command information.
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        ///   Command duration.
        /// </summary>
        public TimeSpan Duration { get; set; }
    }

    /// <summary>
    ///   Contains operation completion data.
    /// </summary>
    public class OperationCompletedEventArgs : InstrumentationEventArgs
    {
        /// <summary>
        ///   Instrumentation context associated with operation.
        /// </summary>
        public IInstrumentationContext Context { get; set; }
    }
}