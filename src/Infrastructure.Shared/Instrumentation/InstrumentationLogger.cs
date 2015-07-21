#region copyright

// Copyright 2013-2014 Alphacloud.Net
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
    using System.Globalization;
    using System.Linq;
    using Core.Data;
    using global::Common.Logging;
    using JetBrains.Annotations;

    public class InstrumentationLogger : IInstrumentationEventListener
    {
        static readonly ILog s_log = LogManager.GetLogger<InstrumentationLogger>();
        readonly LoggingConfiguration _configuration;


        public InstrumentationLogger([NotNull] LoggingConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            _configuration = configuration;
        }

        #region IInstrumentationEventListener Members

        public void CallCompleted(object sender, InstrumentationEventArgs eventArgs)
        {
            var cfg = _configuration.GetCallDurationSettings(eventArgs.CallType);
            if (!cfg.Enabled)
                return;

            var logLevel = LogLevelFromDuration(cfg, eventArgs.Duration);
            s_log.Write(logLevel, m =>
                m("{2} call took {0} ms, operation: '{1}'", eventArgs.Duration.TotalMilliseconds,
                    eventArgs.Info, eventArgs.CallType));
        }


        public void OperationCompleted(object sender, OperationCompletedEventArgs eventArgs)
        {
            if (_configuration.OperationLogging.Enabled)
            {
                int callCount = eventArgs.Context.GetCallCount();
                var logLevel = LogLevelFromCallCount(_configuration.OperationLogging, callCount);
                s_log.Write(logLevel,
                    m =>
                        m("Operation '{0}' completed in {1:0.00} ms, total # of calls: {2}",
                            eventArgs.Info, eventArgs.Duration.TotalMilliseconds, callCount, eventArgs.Info));

                if (_configuration.LogDuplicateCalls)
                {
                    foreach (var callType in eventArgs.Context.GetCallTypes())
                    {
                        var dups = eventArgs.Context.GetDuplicatedCalls(callType)
                            .Select(cs => "'{0}': {1}/{2:0.00} ms".ApplyArgs(cs.Operation, cs.CallCount, cs.Duration.TotalMilliseconds)).ToArray();
                        if (dups.Any())
                        {
                            s_log.InfoFormat(CultureInfo.InvariantCulture, "Duplicate {0} calls {1}: {2}", 
                                callType, dups.Count(), new SequenceFormatter(dups));
                        }
                    }
                }
            }
        }

        #endregion

        static LogLevel LogLevelFromDuration([NotNull] LoggingConfiguration.DurationSettings durationSettings,
            TimeSpan duration)
        {
            if (durationSettings == null) throw new ArgumentNullException("durationSettings");

            var level = LogLevel.Debug;
            if (duration > durationSettings.WarningLevelThreshold)
                level = LogLevel.Warn;
            if (duration > durationSettings.InfoLevelThreshold)
                level = LogLevel.Info;

            return level;
        }


        static LogLevel LogLevelFromCallCount(LoggingConfiguration.CallCounterSettings callCounterSettings,
            int callCount)
        {
            var level = LogLevel.Debug;
            if (callCount > callCounterSettings.WarningLevelThreshold)
                level = LogLevel.Warn;
            if (callCount > callCounterSettings.InfoLevelThreshold)
                level = LogLevel.Info;
            return level;
        }
    }
}