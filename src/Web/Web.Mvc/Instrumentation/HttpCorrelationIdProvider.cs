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
    using System;
    using System.Web;
    using Core.Instrumentation;
    using global::Common.Logging;
    using JetBrains.Annotations;


    /// <summary>
    ///   Per-Http Request Correlation Id provider.
    /// </summary>
    [UsedImplicitly]
    public class HttpCorrelationIdProvider : ThreadStaticCorrelationIdProvider
    {
        static readonly ILog s_log = LogManager.GetLogger<HttpCorrelationIdProvider>();


        /// <summary>
        ///   Initializes a new instance of the <see cref="HttpCorrelationIdProvider" /> class.
        /// </summary>
        /// <param name="diagnosticContext">The diagnostic context.</param>
        public HttpCorrelationIdProvider([NotNull] IDiagnosticContext diagnosticContext)
            : base(diagnosticContext)
        {}


        /// <summary>
        ///   Sets the identifier.
        /// </summary>
        /// <param name="correlationId">The correlation identifier.</param>
        /// <returns></returns>
        public override IDisposable SetId(string correlationId)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items[InstrumentationConstants.CorrelationIdItemKey] = correlationId;
            }
            else
            {
                s_log.Debug("Set: No HttpContext, using thread-local storage");
            }
            return base.SetId(correlationId);
        }


        /// <summary>
        ///   Gets the identifier.
        /// </summary>
        /// <returns></returns>
        public override string GetId()
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Items[InstrumentationConstants.CorrelationIdItemKey] as string;
            }

            s_log.Debug("Get: No HttpContext, using thread-local storage");
            return base.GetId();
        }

        #region Overrides of ThreadStaticCorrelationIdProvider

        /// <summary>
        ///   Clears correlation Id stack.
        /// </summary>
        public override void Clear()
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items[InstrumentationConstants.CorrelationIdItemKey] = null;
            }

            base.Clear();
        }

        #endregion
    }
}