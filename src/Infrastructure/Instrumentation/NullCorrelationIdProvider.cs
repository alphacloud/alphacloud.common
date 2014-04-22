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

namespace Alphacloud.Common.Infrastructure.Instrumentation
{
    using System;
    using Core.Instrumentation;
    using Core.Utils;
    using JetBrains.Annotations;


    /// <summary>
    ///   Null Object implementing <see cref="ICorrelationIdProvider" />.
    /// </summary>
    [PublicAPI]
    public class NullCorrelationIdProvider : ICorrelationIdProvider
    {
        static readonly IDisposable s_disposable = new NullDisposable();

        #region ICorrelationIdProvider Members

        /// <summary>
        ///   Set correlation Id.
        /// </summary>
        /// <param name="correlationId">Correlation id.</param>
        public IDisposable SetId(string correlationId)
        {
            return s_disposable;
        }


        /// <summary>
        ///   Get correlation Id.
        /// </summary>
        /// <returns>Correlation id</returns>
        public string GetId()
        {
            return Guid.NewGuid().ToString("D");
        }


        /// <summary>
        ///   Clear correlation Id.
        /// </summary>
        public void Clear()
        {}

        #endregion
    }
}