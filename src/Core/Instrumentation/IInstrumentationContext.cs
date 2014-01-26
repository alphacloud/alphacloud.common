#region copyright

// Copyright 2014 Alphacloud.Net
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
    ///   Call type.
    /// </summary>
    public static class CallType
    {
        /// <summary>
        ///   Database call.
        /// </summary>
        public const string Database = "Database";

        /// <summary>
        ///   Service call.
        /// </summary>
        public const string Service = "Service";
    }

    /// <summary>
    ///   Method call count info.
    /// </summary>
    public class CallSummary
    {
        /// <summary>
        ///   Method name.
        /// </summary>
        public string Operation { get; set; }

        /// <summary>
        ///   Number of calls made.
        /// </summary>
        public int CallCount { get; set; }

        /// <summary>
        ///   Call type.
        /// </summary>
        public string CallType { get; set; }

        public TimeSpan Duration { get; set; }


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
        ///   Get number of call by type.
        /// </summary>
        /// <param name="callType">Type filter. If <c>null</c>, total number of calls will be returned.</param>
        /// <returns>Number of calls.</returns>
        int GetCallCount(string callType = null);


        /// <summary>
        ///   Calculate total duration of calls.
        /// </summary>
        /// <param name="callType">Type filter. If <c>null</c>, all types will be included.</param>
        /// <returns></returns>
        TimeSpan GetCallDuration(string callType = null);


        /// <summary>
        ///   Find duplicated call by type.
        /// </summary>
        /// <param name="callType">Type filter.</param>
        /// <param name="threshold">Minimum number of duplicates.</param>
        /// <returns>List of duplicated calls.</returns>
        CallSummary[] GetDuplicatedCalls([NotNull] string callType, int threshold = 2);


        /// <summary>
        ///   Add call information.
        /// </summary>
        /// <param name="callType">Type.</param>
        /// <param name="info">Information</param>
        /// <param name="duration">Duration.</param>
        void AddCall(string callType, string info, TimeSpan duration);


        /// <summary>
        ///   Return all captured call types.
        /// </summary>
        /// <returns>Captured call types.</returns>
        string[] GetCallTypes();
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
        ///   Re-initialize context.
        /// </summary>
        void Reset();
    }
}