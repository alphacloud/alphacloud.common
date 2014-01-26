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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using log4net;
    using log4net.Appender;
    using log4net.Core;
    using log4net.Repository.Hierarchy;

    /// <summary>
    ///   Captures log4net messages for specific logger.
    /// </summary>
    /// <remarks>
    ///   Code from http://www.limilabs.com/blog/programmatically-check-log4net
    /// </remarks>
    public class CaptureLog4NetMessages : IDisposable
    {
        readonly MemoryAppender _appender = new MemoryAppender();
        readonly Logger _logger;
        readonly Level _previousLevel;


        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="logName">Log name</param>
        /// <param name="levelToCheck">Logging level to capture on logger.</param>
        public CaptureLog4NetMessages(string logName, Level levelToCheck)
            : this(LogManager.GetLogger(logName), levelToCheck)
        {
        }


        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="logClass">Class to retrieve logger for.</param>
        /// <param name="levelToCheck">Logging level to capture on logger.</param>
        public CaptureLog4NetMessages(Type logClass, Level levelToCheck)
            : this(LogManager.GetLogger(logClass), levelToCheck)
        {
        }


        protected CaptureLog4NetMessages([NotNull] ILog log, [NotNull] Level levelToCheck)
        {
            if (log == null) throw new ArgumentNullException("log");
            if (levelToCheck == null) throw new ArgumentNullException("levelToCheck");

            _logger = (Logger) log.Logger;
            _logger.AddAppender(_appender);
            _previousLevel = _logger.Level;
            _logger.Level = levelToCheck;
        }


        /// <summary>
        ///   Captured messages.
        /// </summary>
        public List<string> Messages
        {
            get
            {
                return new List<LoggingEvent>(
                    _appender.GetEvents()).ConvertAll(x => x.RenderedMessage);
            }
        }

        /// <summary>
        ///   Captured events.
        /// </summary>
        public List<LoggingEvent> Events
        {
            get { return _appender.GetEvents().ToList(); }
        }

        #region IDisposable Members

        public void Dispose()
        {
            _logger.Level = _previousLevel;
            _logger.RemoveAppender(_appender);
        }

        #endregion
    };
}