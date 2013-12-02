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
    using Core.Instrumentation;
    using JetBrains.Annotations;

    /// <summary>
    ///   Instrumentation context provider, based on thread-local storage
    /// </summary>
    [PublicAPI]
    public class ThreadStaticInstrumentationContextProvider : IInstrumentationContextProvider
    {
        [ThreadStatic] static IInstrumentationContext s_instrumentationContext;

        #region IInstrumentationContextProvider Members

        public virtual IInstrumentationContext GetInstrumentationContext()
        {
            var context = GetContextFromThread();
            if (context != null) return context;

            context = new InstrumentationContext();
            SetContextOnThread(context);
            return context;
        }


        public virtual void SetInstrumentationContext(IInstrumentationContext instrumentationContext)
        {
            SetContextOnThread(instrumentationContext);
        }


        public void Reset()
        {
            s_instrumentationContext = null;
        }

        #endregion

        [CanBeNull]
        protected IInstrumentationContext GetContextFromThread()
        {
            return s_instrumentationContext;
        }


        protected void SetContextOnThread(IInstrumentationContext instrumentationContext)
        {
            s_instrumentationContext = instrumentationContext;
        }
    }
}