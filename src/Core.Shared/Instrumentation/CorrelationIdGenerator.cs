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
    using System.Threading;
    using Data;
    using Utils;

    /// <summary>
    ///   Correlation ID generator.
    /// </summary>
    public static class CorrelationIdGenerator
    {
        /// <summary>
        ///   Generate new ID based on current Identity
        /// </summary>
        /// <returns></returns>
        public static string NewId()
        {
            var uid = Guid.NewGuid().ToString("N");
            var identity = Thread.CurrentPrincipal.With(p => p.Identity).Return(i => i.Name, string.Empty);
            identity = identity.Substring(0, Math.Min(20, identity.Length));
            return "{0}-{1}".ApplyArgs(string.IsNullOrWhiteSpace(identity) ? "None" : identity,
                uid.Substring(24));
        }
    }
}