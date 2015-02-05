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

namespace Alphacloud.Common.Instrumentation.Log4Net
{
    using System;
    using Core.Instrumentation;
    using log4net;
    using log4net.Util;

    /// <summary>
    ///   Nested diagnostic context for Log4Net.
    /// </summary>
    public class Log4NetDiagnosticContext : IDiagnosticContext
    {
        static ThreadContextStack Ndc
        {
            get { return LogicalThreadContext.Stacks["NDC"]; }
        }

        #region IDiagnosticContext Members

        /// <summary>
        ///   Set new context.
        /// </summary>
        /// <param name="id">Context Id</param>
        /// <returns>Context handle.</returns>
        public IDisposable Push(string id)
        {
            return Ndc.Push(id);
        }


        /// <summary>
        ///   Pop context.
        /// </summary>
        public void Pop()
        {
            Ndc.Pop();
        }


        /// <summary>
        ///   Clear context.
        /// </summary>
        public void Clear()
        {
            Ndc.Clear();
        }


        /// <summary>
        ///   Gets full context.
        /// </summary>
        /// <returns></returns>
        public string Get()
        {
            return Ndc.ToString();
        }

        #endregion
    }
}