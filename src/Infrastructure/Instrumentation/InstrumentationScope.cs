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
    using System.Diagnostics;
    using Core.Instrumentation;
    using global::Common.Logging;
    using JetBrains.Annotations;

    /// <summary>
    ///   Creates instrumentation operation scope
    /// </summary>
    public class InstrumentationScope : IDisposable
    {
        static readonly ILog s_log = LogManager.GetCurrentClassLogger();

        readonly string _operation;
        readonly IDisposable _correlationId;
        readonly Stopwatch _timer;

        bool _isDisposed;


        public InstrumentationScope(string operation,
            [CanBeNull] IDisposable correlationId = null)
        {
            _operation = operation;
            _correlationId = correlationId;
            SafeServiceLocator.Resolve<IInstrumentationContextProvider>().Reset();

            _timer = Stopwatch.StartNew();
            s_log.InfoFormat("Starting operation '{0}'", _operation);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;
            if (disposing)
            {
                _timer.Stop();

                InstrumentationRuntime.Instance.OnOperationCompleted(this, _operation, _timer.Elapsed);
                SafeServiceLocator.Resolve<IInstrumentationContextProvider>().Reset();

                if (_correlationId != null)
                    _correlationId.Dispose();

#if DEBUG
                // suppress finalization - in debug mode finalizer is used to track leaked scopes.
                GC.SuppressFinalize(this);
#endif
            }
        }


        public void Dispose()
        {
            Dispose(true);
            _isDisposed = true;
        }


#if DEBUG
        // override desctructor in Debug mode only as it affects performance
        ~InstrumentationScope()
        {
            if (!_isDisposed)
                s_log.WarnFormat("Instrumentation scope for '{0}' was not disposed", _operation);
        }
#endif
    }
}