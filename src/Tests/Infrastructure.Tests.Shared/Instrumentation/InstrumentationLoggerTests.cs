#region copyright

// Copyright 2013-2016 Alphacloud.Net
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

// ReSharper disable ExceptionNotDocumented
// ReSharper disable HeapView.ClosureAllocation
// ReSharper disable ExceptionNotDocumentedOptional
// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable HeapView.ObjectAllocation
// ReSharper disable HeapView.ObjectAllocation.Evident
namespace Infrastructure.Tests.Instrumentation
{
    using Alphacloud.Common.Infrastructure.Instrumentation;
    using FluentAssertions;
    using log4net.Core;
    using Logging;
    using NUnit.Framework;

    [TestFixture]
    class InstrumentationLoggerTests
    {
        LoggingConfiguration _configuration;
        InstrumentationContext _instrumentationContext;
        InstrumentationLogger _instrumentationLogger;

        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _configuration = new LoggingConfiguration {OperationLogging = {Enabled = true}};

            _instrumentationLogger = new InstrumentationLogger(_configuration);
            _instrumentationContext = new InstrumentationContext();
        }

        #endregion

        [Test]
        public void CallCompleted_Should_LogCallDuration()
        {
            const string callType = "WebApi";
            _configuration.AddDurationSettings(callType, new LoggingConfiguration.DurationSettings {
                Enabled = true,
                InfoLevelThreshold = 300.Milliseconds(),
                WarningLevelThreshold = 3.Seconds()
            });

            using (var capturedEvents = new CaptureLog4NetEvents(_instrumentationLogger.GetType(), Level.All))
            {
                var eventArgs = new InstrumentationEventArgs {
                    CallType = callType,
                    Duration = 500.Milliseconds(),
                    Info = "query"
                };
                _instrumentationLogger.CallCompleted(this, eventArgs);

                capturedEvents.Events.Should()
                    .Contain(
                        ev =>
                            ev.RenderedMessage == "WebApi call took 500 ms, operation: 'query'" &&
                            ev.Level == Level.Info);
            }
        }


        [Test]
        public void OperationCompleted_Should_LogDuplicatedCalls()
        {
            const string callType1 = "ct1";
            const string callType2 = "ct2";

            _instrumentationContext.AddCall(callType1, "method1", 1.Seconds());
            _instrumentationContext.AddCall(callType1, "method1", 2.Seconds());

            _instrumentationContext.AddCall(callType2, "method1", 3.Seconds());
            _instrumentationContext.AddCall(callType2, "method1", 3.Seconds());
            _instrumentationContext.AddCall(callType2, "method2", 7.Seconds());
            _instrumentationContext.AddCall(callType2, "method2", 9.Seconds());

            _configuration.LogDuplicateCalls = true;
            using (var capturedEvents = new CaptureLog4NetEvents(_instrumentationLogger.GetType(), Level.All))
            {
                _instrumentationLogger.OperationCompleted(this, new OperationCompletedEventArgs {
                    Context = _instrumentationContext,
                    Duration = 1.Seconds(),
                    Info = "operation"
                });

                capturedEvents.Messages.Should()
                    .Contain("Duplicate ct1 calls 1: ['method1': 2/3000.00 ms]");

                capturedEvents.Messages.Should()
                    .Contain("Duplicate ct2 calls 2: ['method1': 2/6000.00 ms, 'method2': 2/16000.00 ms]");
            }
        }


        [Test]
        public void OperationCompleted_Should_LogOperationCompletion()
        {
            using (var capturedEvents = new CaptureLog4NetEvents(_instrumentationLogger.GetType(), Level.All))
            {
                _instrumentationLogger.OperationCompleted(this, new OperationCompletedEventArgs {
                    Context = _instrumentationContext,
                    Duration = 5.Seconds(),
                    Info = "test-operation",
                    ManagedThreadId = 10
                });

                capturedEvents.Messages.Should()
                    .Contain("Operation 'test-operation' completed in 5000.00 ms, total # of calls: 0");
            }
        }
    }
}