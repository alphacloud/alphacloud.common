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

namespace Alphacloud.Common.Core.Instrumentation
{
    using System;
    using System.Collections.Generic;
    using Data;
    using JetBrains.Annotations;

    /// <summary>
    ///   Method call count info.
    /// </summary>
    public class CallCountInfo
    {
        /// <summary>
        ///   Method name.
        /// </summary>
        public string Operation { get; set; }

        /// <summary>
        ///   Number of calls made.
        /// </summary>
        public int CallCount { get; set; }


        public override string ToString()
        {
            return "{0}: {1}".ApplyArgs(Operation, CallCount);
        }
    }


    /// <summary>
    ///   Represents Instrumentation context.
    ///   All methods must be thread-safe.
    /// </summary>
    [PublicAPI]
    public interface IInstrumentationContext
    {
        /// <summary>
        ///   Number of database calls made.
        /// </summary>
        int DatabaseCallCount { get; }

        /// <summary>
        ///   Total time spent waiting for database calls to complete.
        /// </summary>
        TimeSpan DatabaseCallsDuration { get; }

        /// <summary>
        ///   Number of external Service calls made.
        /// </summary>
        int ServiceCallCount { get; }

        /// <summary>
        ///   Total time spent waiting for external service calls to complete.
        /// </summary>
        TimeSpan ServiceCallsDuration { get; }


        /// <summary>
        ///   Total number of calls made.
        /// </summary>
        /// <returns>Number of DB and service calls.</returns>
        int GetTotalCallCount();


        /// <summary>
        ///   Add database call statistics.
        /// </summary>
        /// <param name="command">SQL command</param>
        /// <param name="duration">Duration.</param>
        void AddDatabaseCall(string command, TimeSpan duration);


        /// <summary>
        ///   Add external service call sattistics.
        /// </summary>
        /// <param name="serviceMethod">Service method being called.</param>
        /// <param name="duration">Duration.</param>
        void AddServiceCall(string serviceMethod, TimeSpan duration);


        /// <summary>
        ///   Returns duplicated database calls.
        /// </summary>
        /// <param name="threshold">The threshold.</param>
        /// <returns></returns>
        IList<CallCountInfo> GetDuplicatedDbCalls(int threshold);


        /// <summary>
        ///   Returns duplicated service calls.
        /// </summary>
        /// <param name="threshold">The threshold.</param>
        /// <returns></returns>
        IList<CallCountInfo> GetDuplicatedServiceCalls(int threshold);
    }

    /// <summary>
    ///   Instrumentation Context provider.
    /// </summary>
    public interface IInstrumentationContextProvider
    {
        /// <summary>
        ///   Returns current instrumentation context.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        IInstrumentationContext GetInstrumentationContext();


        /// <summary>
        ///   Set current instrumentation context.
        /// </summary>
        /// <param name="instrumentationContext"></param>
        void SetInstrumentationContext([CanBeNull] IInstrumentationContext instrumentationContext);


        /// <summary>
        /// Re-initialize context.
        /// </summary>
        void Reset();
    }
}