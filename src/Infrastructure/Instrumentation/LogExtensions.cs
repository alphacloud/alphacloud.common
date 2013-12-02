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
    using System.Globalization;
    using global::Common.Logging;
    using JetBrains.Annotations;

    /// <summary>
    ///   Logging extensions.
    /// </summary>
    public static class LogExtensions
    {
        /// <summary>
        ///   Write message with desired log level.
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="logLevel">Log level</param>
        /// <param name="formatMessageCallback">Message formatter.</param>
        public static void Write([NotNull] this ILog log, LogLevel logLevel,
            [NotNull] Action<FormatMessageHandler> formatMessageCallback)
        {
            if (log == null) throw new ArgumentNullException("log");
            if (formatMessageCallback == null) throw new ArgumentNullException("formatMessageCallback");

            switch (logLevel)
            {
                case LogLevel.Debug:
                    log.Debug(CultureInfo.InvariantCulture, formatMessageCallback);
                    break;
                case LogLevel.Info:
                    log.Info(CultureInfo.InvariantCulture, formatMessageCallback);
                    break;
                case LogLevel.Warn:
                    log.Warn(CultureInfo.InvariantCulture, formatMessageCallback);
                    break;
                case LogLevel.Error:
                    log.Error(CultureInfo.InvariantCulture, formatMessageCallback);
                    break;
                case LogLevel.Fatal:
                    log.Fatal(CultureInfo.InvariantCulture, formatMessageCallback);
                    break;
            }
        }
    }
}