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

namespace Alphacloud.Common.Infrastructure.Caching
{
    using System.Threading;
    using Core.Data;
    using Transformations;

    /// <summary>
    ///   Define cache scope based on curent User Identity.
    /// </summary>
    public class UserIdentityCacheKeyScope : IEncoder<string>
    {
        #region IEncoder<string> Members

        /// <summary>
        ///   Prefix key with current identity name.
        /// </summary>
        /// <param name="source">Cache key</param>
        /// <returns>Cache key prefixed with current thread user name</returns>
        public string Encode(string source)
        {
            return "{0}.{1}".ApplyArgs(Thread.CurrentPrincipal.Identity.Name, source);
        }

        #endregion
    }
}