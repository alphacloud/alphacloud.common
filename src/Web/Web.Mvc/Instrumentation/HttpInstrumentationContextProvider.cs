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

namespace Alphacloud.Common.Web.Mvc.Instrumentation
{
    using System.Web;
    using Core.Instrumentation;
    using global::Common.Logging;
    using Infrastructure.Instrumentation;


    /// <summary>
    ///   Instrumentation Context provider to use with ASP.NET applications.
    ///   Manages per-request instrumentation context.
    /// </summary>
    public class HttpInstrumentationContextProvider : ThreadStaticInstrumentationContextProvider
    {
        const string ContextKey = "Alphacloud.Instrumentation.Context";
        static readonly ILog s_log = LogManager.GetCurrentClassLogger();


        /// <summary>
        ///   Gets the instrumentation context.
        /// </summary>
        /// <returns></returns>
        public override IInstrumentationContext GetInstrumentationContext()
        {
            var httpContext = HttpContext.Current;
            if (httpContext == null)
            {
                s_log.Debug("Get: No HttpContext, using thread-local storage.");
                return base.GetInstrumentationContext();
            }
            var context = httpContext.Items[ContextKey] as IInstrumentationContext;
            if (context == null)
            {
                context = new InstrumentationContext();
                httpContext.Items[ContextKey] = context;
            }
            return context;
        }


        /// <summary>
        ///   Sets the instrumentation context.
        /// </summary>
        /// <param name="instrumentationContext">The instrumentation context.</param>
        public override void SetInstrumentationContext(IInstrumentationContext instrumentationContext)
        {
            var httpContext = HttpContext.Current;
            if (httpContext == null)
            {
                s_log.Debug("Set: No HttpContext, using thread-local storage.");
                base.SetInstrumentationContext(instrumentationContext);
                return;
            }
            httpContext.Items[ContextKey] = instrumentationContext;
        }
    }
}