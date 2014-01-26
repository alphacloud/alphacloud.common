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
        static readonly ILog s_log = LogManager.GetCurrentClassLogger();
        readonly LoggingConfiguration _configuration;


        public InstrumentationLogger([NotNull] LoggingConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            _configuration = configuration;
        }


        public void DatabaseCallCompleted(object sender, InstrumentationEventArgs eventArgs)
        {
            if (!_configuration.DatabaseCallLogging.Enabled)
                return;

            var logLevel = LogLevelFromDuration(_configuration.DatabaseCallLogging, eventArgs.Duration);
            s_log.Write(logLevel, m =>
                m("Database call took {0} ms; Sql: '{1}'", eventArgs.Duration.TotalMilliseconds, eventArgs.Command));
        }


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


        public void ServiceCallCompleted(object sender, InstrumentationEventArgs eventArgs)
        {
            if (!_configuration.ServiceCallLogging.Enabled)
                return;

            var logLevel = LogLevelFromDuration(_configuration.ServiceCallLogging, eventArgs.Duration);
            s_log.Write(logLevel, m =>
                m("Service call took {0} ms; Service method: '{1}'", eventArgs.Duration.TotalMilliseconds,
                    eventArgs.Command));
        }


        public void OperationCompleted(object sender, OperationCompletedEventArgs eventArgs)
        {
            if (_configuration.OperationLogging.Enabled)
            {
                int callCount = eventArgs.Context.GetTotalCallCount();
                var logLevel = LogLevelFromCallCount(_configuration.OperationLogging, callCount);
                s_log.Write(logLevel,
                    m =>
                        m("Operation completed in {0} ms, # of calls: {1} Operation: '{2}'", eventArgs.Duration,
                            callCount, eventArgs.Context));

                if (_configuration.LogDuplicateCalls)
                {
                    var dbDups = eventArgs.Context.GetDuplicatedDbCalls(1)
                        .Select(ci => "'{0}': {1}".ApplyArgs(ci.Operation, ci.CallCount)).ToArray();
                    var svcDups = eventArgs.Context.GetDuplicatedServiceCalls(1)
                        .Select(ci => "'{0}': {1}".ApplyArgs(ci.Operation, ci.CallCount)).ToArray();
                    if (dbDups.Any())
                    {
                        s_log.InfoFormat(CultureInfo.InvariantCulture, "Duplicated database calls - {0}: '{1}",
                            new SequenceFormatter(dbDups));
                    }
                    if (svcDups.Any())
                    {
                        s_log.InfoFormat(CultureInfo.InvariantCulture, "Duplicated service calls = {0}: '{1}'",
                            new SequenceFormatter(svcDups));
                    }
                }
            }
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