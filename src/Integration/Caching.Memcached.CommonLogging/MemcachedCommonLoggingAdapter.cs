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

namespace Alphacloud.Common.Caching.Memcached.CommonLogging
{
    using System;
    using System.Globalization;
    using Enyim.Caching;
    using JetBrains.Annotations;

    /// <summary>
    ///   CommonLogging adapter for Memcached.
    /// </summary>
    [PublicAPI]
    public class MemcachedCommonLoggingAdapter : ILog
    {
        readonly global::Common.Logging.ILog _log;


        /// <summary>
        ///   Initializes a new instance of the <see cref="MemcachedCommonLoggingAdapter" /> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <exception cref="System.ArgumentNullException">log</exception>
        public MemcachedCommonLoggingAdapter([NotNull] global::Common.Logging.ILog log)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            _log = log;
        }

        #region ILog Members

        /// <summary>
        ///   Log debug message.
        /// </summary>
        /// <param name="message">Message.</param>
        public void Debug(object message)
        {
            _log.Debug(message);
        }


        /// <summary>
        ///   Log debug message with exception.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="exception">Exception.</param>
        public void Debug(object message, Exception exception)
        {
            _log.Debug(message, exception);
        }


        /// <summary>
        ///   Log formatted debug message using invariant culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="arg0">Argument 0.</param>
        [StringFormatMethod("format")]
        public void DebugFormat(string format, object arg0)
        {
            _log.DebugFormat(CultureInfo.InvariantCulture, format, arg0);
        }


        /// <summary>
        ///   Log formatted debug message using invariant culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="arg0">Argument 0.</param>
        /// <param name="arg1">Argument 1.</param>
        [StringFormatMethod("format")]
        public void DebugFormat(string format, object arg0, object arg1)
        {
            _log.DebugFormat(CultureInfo.InvariantCulture, format, arg0, arg1);
        }


        /// <summary>
        ///   Log formatted debug message using invariant culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="arg0">Argument 0.</param>
        /// <param name="arg1">Argument 1.</param>
        /// <param name="arg2">Argument 2.</param>
        [StringFormatMethod("format")]
        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            _log.DebugFormat(CultureInfo.InvariantCulture, format, arg0, arg1, arg2);
        }


        /// <summary>
        ///   Log formatted debug message using invariant culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="args">Arguments.</param>
        [StringFormatMethod("format")]
        public void DebugFormat(string format, params object[] args)
        {
            _log.DebugFormat(CultureInfo.InvariantCulture, format, args);
        }


        /// <summary>
        ///   Log formatted debug message using custom format provider.
        /// </summary>
        /// <param name="provider">Format provider.</param>
        /// <param name="format">Format specifier.</param>
        /// <param name="args">Arguments.</param>
        [StringFormatMethod("format")]
        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.DebugFormat(provider, format, args);
        }


        /// <summary>
        ///   Log information message.
        /// </summary>
        /// <param name="message">Message.</param>
        public void Info(object message)
        {
            _log.Info(message);
        }


        /// <summary>
        ///   Log information message with exception.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="exception">Exception.</param>
        public void Info(object message, Exception exception)
        {
            _log.Info(message, exception);
        }


        /// <summary>
        ///   Log formatted information message using invariant culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="arg0">Argument 0.</param>
        [StringFormatMethod("format")]
        public void InfoFormat(string format, object arg0)
        {
            _log.InfoFormat(CultureInfo.InvariantCulture, format, arg0);
        }


        /// <summary>
        ///   Log formatted information message using invariant culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="arg0">Argument 0.</param>
        /// <param name="arg1">Argument 1.</param>
        [StringFormatMethod("format")]
        public void InfoFormat(string format, object arg0, object arg1)
        {
            _log.InfoFormat(CultureInfo.InvariantCulture, format, arg0, arg1);
        }


        /// <summary>
        ///   Log formatted information message using invariant culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="arg0">Argument 0.</param>
        /// <param name="arg1">Argument 1.</param>
        /// <param name="arg2">Argument 2.</param>
        [StringFormatMethod("format")]
        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            _log.InfoFormat(CultureInfo.InvariantCulture, format, arg0, arg1, arg2);
        }


        /// <summary>
        ///   Log formatted information message using invariant culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="args">Arguments.</param>
        [StringFormatMethod("format")]
        public void InfoFormat(string format, params object[] args)
        {
            _log.InfoFormat(CultureInfo.InvariantCulture, format, args);
        }


        /// <summary>
        ///   Log formatted information message using custom formatter.
        /// </summary>
        /// <param name="provider">format provider.</param>
        /// <param name="format">Format specifier.</param>
        /// <param name="args">Arguments.</param>
        [StringFormatMethod("format")]
        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.InfoFormat(provider, format, args);
        }


        /// <summary>
        ///   Log warning message.
        /// </summary>
        /// <param name="message">Message.</param>
        public void Warn(object message)
        {
            _log.Warn(message);
        }


        /// <summary>
        ///   Log warning message with exception.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="exception">Exceptin.</param>
        public void Warn(object message, Exception exception)
        {
            _log.Warn(message, exception);
        }


        /// <summary>
        ///   Log formatted warning message using invariant culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="arg0">Argument 0.</param>
        [StringFormatMethod("format")]
        public void WarnFormat(string format, object arg0)
        {
            _log.WarnFormat(CultureInfo.InvariantCulture, format, arg0);
        }


        /// <summary>
        ///   Log formatted warning message using invariant culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="arg0">Argument 0.</param>
        /// <param name="arg1">Argument 1.</param>
        [StringFormatMethod("format")]
        public void WarnFormat(string format, object arg0, object arg1)
        {
            _log.WarnFormat(CultureInfo.InvariantCulture, format, arg0, arg1);
        }


        /// <summary>
        ///   Log formatted warning message using invariant culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="arg0">Argument 0.</param>
        /// <param name="arg1">Argument 1.</param>
        /// <param name="arg2">Argument 2.</param>
        [StringFormatMethod("format")]
        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            _log.WarnFormat(CultureInfo.InvariantCulture, format, arg0, arg1, arg2);
        }


        /// <summary>
        ///   Log formatted warning message using invariant culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="args">Arguments.</param>
        [StringFormatMethod("format")]
        public void WarnFormat(string format, params object[] args)
        {
            _log.WarnFormat(CultureInfo.InvariantCulture, format, args);
        }


        /// <summary>
        ///   Log formatted warning message using custom formatter.
        /// </summary>
        /// <param name="provider">Format provider.</param>
        /// <param name="format">Format specifier.</param>
        /// <param name="args">Arguments.</param>
        [StringFormatMethod("format")]
        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.WarnFormat(provider, format, args);
        }


        /// <summary>
        ///   Log error message.
        /// </summary>
        /// <param name="message">Message.</param>
        public void Error(object message)
        {
            _log.Error(message);
        }


        /// <summary>
        ///   Log error message with exception.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="exception">Exception.</param>
        public void Error(object message, Exception exception)
        {
            _log.Error(message, exception);
        }


        /// <summary>
        ///   Log formatted error message using invariant culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="arg0">Argument 0.</param>
        [StringFormatMethod("format")]
        public void ErrorFormat(string format, object arg0)
        {
            _log.ErrorFormat(CultureInfo.InvariantCulture, format, arg0);
        }


        /// <summary>
        ///   Log formatted error message using invariant culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="arg0">Argument 0.</param>
        /// <param name="arg1">Argument 1.</param>
        [StringFormatMethod("format")]
        public void ErrorFormat(string format, object arg0, object arg1)
        {
            _log.ErrorFormat(CultureInfo.InvariantCulture, format, arg0, arg1);
        }


        /// <summary>
        ///   Log formatted error message using invariant culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="arg0">Argument 0.</param>
        /// <param name="arg1">Argument 1.</param>
        /// <param name="arg2">Argument 2.</param>
        [StringFormatMethod("format")]
        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            _log.ErrorFormat(CultureInfo.InvariantCulture, format, arg0, arg1, arg2);
        }


        /// <summary>
        ///   Log formatted warning message using invariant culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="args">Arguments.</param>
        [StringFormatMethod("format")]
        public void ErrorFormat(string format, params object[] args)
        {
            _log.ErrorFormat(CultureInfo.InvariantCulture, format, args);
        }


        /// <summary>
        ///   Log formatted warning message using custom formatter.
        /// </summary>
        /// <param name="provider">Format provider.</param>
        /// <param name="format">Format specifier.</param>
        /// <param name="args">Arguments.</param>
        [StringFormatMethod("format")]
        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.ErrorFormat(provider, format, args);
        }


        /// <summary>
        ///   Log fatal message.
        /// </summary>
        /// <param name="message">Message.</param>
        public void Fatal(object message)
        {
            _log.Fatal(message);
        }


        /// <summary>
        ///   Log fatal message with exception.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="exception">Exception.</param>
        public void Fatal(object message, Exception exception)
        {
            _log.Fatal(message, exception);
        }


        /// <summary>
        ///   Log formatted fatal message using invariant culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="arg0">Argument 0.</param>
        [StringFormatMethod("format")]
        public void FatalFormat(string format, object arg0)
        {
            _log.FatalFormat(CultureInfo.InvariantCulture, format, arg0);
        }


        /// <summary>
        ///   Log formatted fatal message using invariant culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="arg0">Argument 0.</param>
        /// <param name="arg1">Argument 1.</param>
        [StringFormatMethod("format")]
        public void FatalFormat(string format, object arg0, object arg1)
        {
            _log.FatalFormat(CultureInfo.InvariantCulture, format, arg0, arg1);
        }


        /// <summary>
        ///   Log formatted fatal message using invariant culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="arg0">Argument 0.</param>
        /// <param name="arg1">Argument 1.</param>
        /// <param name="arg2">Argument 2.</param>
        [StringFormatMethod("format")]
        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            _log.FatalFormat(CultureInfo.InvariantCulture, format, arg0, arg1, arg2);
        }


        /// <summary>
        ///   Log formatted fatal message using invariant culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="args">Arguments.</param>
        [StringFormatMethod("format")]
        public void FatalFormat(string format, params object[] args)
        {
            _log.FatalFormat(CultureInfo.InvariantCulture, format, args);
        }


        /// <summary>
        ///   Log formatted fatal message using custom format provider.
        /// </summary>
        /// <param name="provider">Format provider.</param>
        /// <param name="format">Format specifier.</param>
        /// <param name="args">Arguments.</param>
        [StringFormatMethod("format")]
        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.FatalFormat(provider, format, args);
        }


        /// <summary>
        ///   Checks if logger is enabled for Debug level.
        /// </summary>
        public bool IsDebugEnabled
        {
            get { return _log.IsDebugEnabled; }
        }


        /// <summary>
        ///   Checks if logger is enabled for Information level.
        /// </summary>
        public bool IsInfoEnabled
        {
            get { return _log.IsInfoEnabled; }
        }


        /// <summary>
        ///   Checks if logger is enabled for Warning level.
        /// </summary>
        public bool IsWarnEnabled
        {
            get { return _log.IsWarnEnabled; }
        }


        /// <summary>
        ///   Checks if logger is enabled for Error level.
        /// </summary>
        public bool IsErrorEnabled
        {
            get { return _log.IsErrorEnabled; }
        }


        /// <summary>
        ///   Checks if logger is enabled for Fatal level.
        /// </summary>
        public bool IsFatalEnabled
        {
            get { return _log.IsFatalEnabled; }
        }

        #endregion
    }
}