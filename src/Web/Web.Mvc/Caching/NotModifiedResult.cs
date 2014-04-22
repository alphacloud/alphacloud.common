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

namespace Alphacloud.Common.Web.Mvc.Caching
{
    using System.Net;
    using System.Web.Mvc;
    using JetBrains.Annotations;


    /// <summary>
    ///   Represents HTTP 304 (Not Modified) response.
    /// </summary>
    [PublicAPI]
    public class NotModifiedResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = (int) HttpStatusCode.NotModified;
            response.StatusDescription = "Not Modified.";
            response.SuppressContent = true;
        }

        #region Overrides of Object

        /// <summary>
        /// Returns human readable status for HTTP 304.
        /// </summary>
        public override string ToString()
        {
            return "HTTP 304 Not Modified.";
        }

        #endregion
    }
}