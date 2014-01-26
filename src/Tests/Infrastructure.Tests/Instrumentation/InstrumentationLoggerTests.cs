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
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _configuration = new LoggingConfiguration {OperationLogging = {Enabled = true}};

            _instrumentationLogger = new InstrumentationLogger(_configuration);
            _instrumentationContext = new InstrumentationContext();
        }

        #endregion

        LoggingConfiguration _configuration;
        InstrumentationLogger _instrumentationLogger;
        InstrumentationContext _instrumentationContext;


        [Test]
        public void OperationCompleted_Should_LogOperationCompletion()
        {
            using (var capturedLog = new CaptureLog4NetMessages(_instrumentationLogger.GetType(), Level.All))
            {
                _instrumentationLogger.OperationCompleted(this, new OperationCompletedEventArgs {
                    Context = _instrumentationContext,
                    Duration = 5.Seconds(),
                    Info = "test-operation",
                    ManagedThreadId = 10
                });

                capturedLog.Messages.Should()
                    .Contain("Operation 'test-operation' completed in 5,000.00 ms, total # of calls: 0");
            }
        }
    }
}