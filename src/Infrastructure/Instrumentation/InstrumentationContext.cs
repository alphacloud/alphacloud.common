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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Core.Instrumentation;
    using global::Common.Logging;

    /// <summary>
    ///   Call information.
    /// </summary>
    public class CallInfo
    {
        public CallInfo(string operation, TimeSpan duration, int callingThreadId)
        {
            Operation = operation;
            Duration = duration;
            CallingThreadId = callingThreadId;
        }


        public string Operation { get; private set; }
        public TimeSpan Duration { get; private set; }

        public int CallingThreadId { get; private set; }
    }

    /// <summary>
    ///   Instrumentation context implementation.
    /// </summary>
    [Serializable]
    public class InstrumentationContext : IInstrumentationContext
    {
        static readonly ILog s_log = LogManager.GetCurrentClassLogger();

        readonly List<CallInfo> _dbCalls = new List<CallInfo>();
        readonly List<CallInfo> _serviceCalls = new List<CallInfo>();


        /// <summary>
        ///   Initializes a new instance of the <see cref="InstrumentationContext" /> class.
        /// </summary>
        public InstrumentationContext()
        {
            s_log.Debug("New InstrumentationContext created.");
        }

        #region IInstrumentationContext Members

        public int GetTotalCallCount()
        {
            lock (this)
            {
                return _dbCalls.Count + _serviceCalls.Count;
            }
        }


        public int DatabaseCallCount
        {
            get
            {
                lock (this)
                {
                    return _dbCalls.Count;
                }
            }
        }

        public TimeSpan DatabaseCallsDuration
        {
            get
            {
                lock (this)
                {
                    return CalculateDuration(_dbCalls);
                }
            }
        }


        public int ServiceCallCount
        {
            get
            {
                lock (this)
                {
                    return _serviceCalls.Count;
                }
            }
        }

        public TimeSpan ServiceCallsDuration
        {
            get
            {
                lock (this)
                {
                    return CalculateDuration(_serviceCalls);
                }
            }
        }


        public void AddDatabaseCall(string command, TimeSpan duration)
        {
            int callCnt;
            lock (this)
            {
                _dbCalls.Add(new CallInfo(command, duration, Thread.CurrentThread.ManagedThreadId));
                callCnt = _dbCalls.Count;
            }
            s_log.Debug(m => m("Database call: {0}, {1:#0.0} ms; total call count={2}",
                command, duration.TotalMilliseconds, callCnt));
        }


        public void AddServiceCall(string serviceMethod, TimeSpan duration)
        {
            int callCnt;
            lock (this)
            {
                _serviceCalls.Add(new CallInfo(serviceMethod, duration, Thread.CurrentThread.ManagedThreadId));
                callCnt = _serviceCalls.Count;
            }
            s_log.Debug(m => m("Service call: {0}, {1:#0.0} ms; total call count={2}",
                serviceMethod, duration.TotalMilliseconds, callCnt));
        }


        public IList<CallCountInfo> GetDuplicatedDbCalls(int threshold)
        {
            if (threshold <= 0)
                throw new ArgumentOutOfRangeException("threshold", threshold, @"Threshold should be positive number");
            lock (this)
            {
                return CollectDuplicates(_dbCalls, threshold);
            }
        }


        public IList<CallCountInfo> GetDuplicatedServiceCalls(int threshold)
        {
            if (threshold <= 0)
                throw new ArgumentOutOfRangeException("threshold", threshold, @"Threshold should be positive number");
            lock (this)
            {
                return CollectDuplicates(_serviceCalls, threshold);
            }
        }

        #endregion

        TimeSpan CalculateDuration(IList<CallInfo> callInfo)
        {
            var duration = TimeSpan.Zero;
            for (int i = 0; i < callInfo.Count; i++)
            {
                duration += callInfo[i].Duration;
            }
            return duration;
        }


        static IList<CallCountInfo> CollectDuplicates(IEnumerable<CallInfo> callInfo, int threshold)
        {
            var calls = from call in callInfo
                group call by call.Operation
                into callCount
                select new CallCountInfo {
                    Operation = callCount.Key,
                    CallCount = callCount.Count()
                };
            return calls.Where(cc => cc.CallCount >= threshold).ToList();
        }
    }
}