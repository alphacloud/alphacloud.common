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
    using System.Web.Mvc;
    using JetBrains.Annotations;


    /// <summary>
    ///   Instruct the client browser to store responses in the History folder.
    /// </summary>
    [PublicAPI]
    public sealed class ExcludeFromBrowserHistory : ActionFilterAttribute
    {
        #region Overrides of ActionFilterAttribute


        /// <summary>
        /// Sets HTTP cache properties.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetAllowResponseInBrowserHistory(true);
            base.OnResultExecuting(filterContext);
        }

        #endregion
    }
}