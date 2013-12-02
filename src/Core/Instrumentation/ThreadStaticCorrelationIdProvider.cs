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

namespace Alphacloud.Common.Core.Instrumentation
{
    using System;
    using JetBrains.Annotations;

    public class ThreadStaticCorrelationIdProvider : ICorrelationIdProvider
    {
        [ThreadStatic] static string s_correlationId;

        readonly IDiagnosticContext _diagnosticContext;


        public ThreadStaticCorrelationIdProvider([NotNull] IDiagnosticContext diagnosticContext)
        {
            if (diagnosticContext == null)
            {
                throw new ArgumentNullException("diagnosticContext");
            }
            _diagnosticContext = diagnosticContext;
        }

        #region ICorrelationIdProvider Members

        public virtual IDisposable SetId(string correlationId)
        {
            s_correlationId = correlationId;
            return SetNdc(correlationId);
        }


        public virtual string GetId()
        {
            return s_correlationId;
        }


        public virtual void Clear()
        {
            s_correlationId = null;
            _diagnosticContext.Clear();
        }

        #endregion

        protected IDisposable SetNdc(string correlationId)
        {
            return _diagnosticContext.Push(correlationId);
        }
    }
}