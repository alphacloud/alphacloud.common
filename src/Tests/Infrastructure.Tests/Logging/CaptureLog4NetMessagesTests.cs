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

namespace Infrastructure.Tests.Logging
{
    using Common.Logging;
    using FluentAssertions;
    using log4net.Appender;
    using log4net.Core;
    using log4net.Repository.Hierarchy;
    using NUnit.Framework;

    [TestFixture]
    public class CaptureLog4NetMessagesTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            AddLoggingAppender();
        }


        [TearDown]
        public void TearDown()
        {
            if (_logger != null)
                _logger.RemoveAppender(_memoryAppender);
        }

        #endregion

        static readonly ILog s_log = LogManager.GetLogger<CaptureLog4NetEvents>(); // initialize log system.


        MemoryAppender _memoryAppender;
        IAppenderAttachable _logger;


        void AddLoggingAppender()
        {
            var root = ((Hierarchy) log4net.LogManager.GetRepository()).Root;
            _logger = root;
            _logger.Should().NotBeNull("cannot attach log appender");

            _memoryAppender = new MemoryAppender();

            _logger.AddAppender(_memoryAppender);
        }


        [Test]
        public void CanCaptureLogMessage()
        {
            var log = LogManager.GetLogger("InstrumentationLoggerTests");
            log.Info("Captured info message");

            var messages = _memoryAppender.GetEvents();
            messages.Should().Contain(m => m.RenderedMessage == "Captured info message");
        }


        [Test]
        public void LogChecker_Should_CaptureLogMessages()
        {
            using (var lc = new CaptureLog4NetEvents(GetType(), Level.All))
            {
                const string testMessage = @"test- 12648923654";
                s_log.Info(testMessage);

                lc.Messages.Should().Contain(testMessage);
            }
        }
    }
}