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

namespace Alphacloud.Common.Infrastructure.Instrumentation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Core.Instrumentation;
    using global::Common.Logging;
    using JetBrains.Annotations;

    /// <summary>
    ///   Call information.
    /// </summary>
    [Serializable]
    public class CallInfo
    {
        public CallInfo([NotNull] string type, [NotNull] string operation, TimeSpan duration, int callingThreadId)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (operation == null) throw new ArgumentNullException("operation");

            CallType = string.Intern(type);
            Operation = operation;
            Duration = duration;
            CallingThreadId = callingThreadId;
        }


        public string CallType { get; private set; }
        public string Operation { get; private set; }
        public TimeSpan Duration { get; private set; }
        public int CallingThreadId { get; private set; }
    }

    /// <summary>
    ///   Instrumentation context implementation.
    /// </summary>
    [Serializable]
    public sealed class InstrumentationContext : IInstrumentationContext
    {
        static readonly ILog s_log = LogManager.GetLogger(InstrumentationConstants.Context);
        readonly List<CallInfo> _calls = new List<CallInfo>();


        /// <summary>
        ///   Initializes a new instance of the <see cref="InstrumentationContext" /> class.
        /// </summary>
        public InstrumentationContext()
        {
            s_log.Debug("New InstrumentationContext created.");
        }

        #region IInstrumentationContext Members

        public int GetCallCount(string callType = null)
        {
            lock (_calls)
            {
                return FilterByCallType(callType).Count();
            }
        }


        public TimeSpan GetCallDuration(string callType = null)
        {
            lock (_calls)
            {
                return FilterByCallType(callType).Aggregate(TimeSpan.Zero, (current, call) => current + call.Duration);
            }
        }


        public void AddCall(string callType, string info, TimeSpan duration)
        {
            int callCnt;
            lock (this)
            {
                _calls.Add(new CallInfo(callType, info, duration, Thread.CurrentThread.ManagedThreadId));
                callCnt = _calls.Count;
            }

            if (s_log.IsDebugEnabled)
            {
                s_log.DebugFormat(CultureInfo.InvariantCulture,
                    "'{3}' call: '{0}', {1:#0.0} ms; current call count= {2}",
                    info, duration.TotalMilliseconds, callCnt, callType);
            }
        }


        public string[] GetCallTypes()
        {
            lock (_calls)
            {
                return _calls.Select(c => c.CallType).Distinct().ToArray();
            }
        }


        public CallSummary[] GetDuplicatedCalls([NotNull] string callType, int threshold = 2)
        {
            if (callType == null) throw new ArgumentNullException("callType");
            lock (_calls)
            {
                return CollectDuplicates(_calls, threshold, callType).ToArray();
            }
        }

        #endregion

        static IEnumerable<CallSummary> CollectDuplicates(IEnumerable<CallInfo> callInfo, int threshold,
            string callType)
        {
            callType = string.Intern(callType);
            return (from ci in callInfo
                where ci.CallType == callType
                group ci by ci.Operation
                into key
                select new CallSummary {
                    Operation = key.Key,
                    CallType = callType,
                    CallCount = key.Count(),
                    Duration = key.Aggregate(TimeSpan.Zero, (current, c) => current += c.Duration)
                })
                .Where(callSummary => callSummary.CallCount >= threshold);
        }


        IEnumerable<CallInfo> FilterByCallType(string callType)
        {
            var ct = callType != null ? string.Intern(callType) : null;
            var calls = ct != null ? _calls.Where(c => c.CallType == ct) : _calls;
            return calls;
        }
    }
}