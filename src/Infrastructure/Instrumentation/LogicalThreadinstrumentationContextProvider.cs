#region copyright

// Copyright 2013-2015 Alphacloud.Net
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
    using System.Runtime.Remoting.Messaging;
    using Core.Instrumentation;
    using JetBrains.Annotations;

    /// <summary>
    ///   LogicalThread-based instrumentation context storage.
    /// </summary>
    [PublicAPI]
    public class LogicalThreadInstrumentationContextProvider : IInstrumentationContextProvider
    {
        internal const string ContextSlotName = "alphacloud.common.instrumentation.context";

        #region IInstrumentationContextProvider Members

        /// <summary>
        ///   Returns current instrumentation context.
        /// </summary>
        /// <returns></returns>
        public IInstrumentationContext GetInstrumentationContext()
        {
            var context = CallContext.LogicalGetData(ContextSlotName) as IInstrumentationContext;
            if (context != null)
                return context;

            context = new InstrumentationContext();
            SetInstrumentationContext(context);
            return context;
        }


        /// <summary>
        ///   Set current instrumentation context.
        /// </summary>
        /// <param name="instrumentationContext"></param>
        public void SetInstrumentationContext(IInstrumentationContext instrumentationContext)
        {
            CallContext.LogicalSetData(ContextSlotName, instrumentationContext);
        }


        /// <summary>
        ///   Re-initialize context.
        /// </summary>
        public void Reset()
        {
            SetInstrumentationContext(new InstrumentationContext());
        }

        #endregion
    }
}