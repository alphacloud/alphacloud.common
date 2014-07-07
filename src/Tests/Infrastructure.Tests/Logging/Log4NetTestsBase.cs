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
    using FluentAssertions;
    using log4net;
    using log4net.Appender;
    using log4net.Core;
    using log4net.Repository.Hierarchy;
    using NUnit.Framework;

    /// <summary>
    ///   Base class for testing log4net logging.
    ///   <para>
    ///     Created MemoryAppender to catch all logging messages.
    ///     Use <see cref="CaptureLog4NetEvents" /> to capture specific logger messages.
    ///   </para>
    /// </summary>
    /// <remarks>
    ///   <seealso cref="CaptureLog4NetMessagesTests" />
    /// </remarks>
    public class Log4NetTestsBase
    {
        /// <summary>
        /// </summary>
        protected MemoryAppender MemoryAppender { get; set; }

        protected IAppenderAttachable Logger { get; private set; }


        [SetUp]
        public void SetUp()
        {
            AddLoggingAppender();
            DoSetUp();
        }


        /// <summary>
        ///   Perform additional setup logic.
        /// </summary>
        protected virtual void DoSetUp()
        {
        }


        [TearDown]
        public void TearDown()
        {
            if (Logger != null)
                Logger.RemoveAppender(MemoryAppender);
            DoTearDown();
        }


        /// <summary>
        ///   Perform additional cleanup logic.
        /// </summary>
        protected virtual void DoTearDown()
        {
        }


        void AddLoggingAppender()
        {
            var root = ((Hierarchy) LogManager.GetRepository()).Root;
            Logger = root;
            Logger.Should().NotBeNull("cannot attach log appender");

            MemoryAppender = new MemoryAppender();

            Logger.AddAppender(MemoryAppender);
        }
    }
}