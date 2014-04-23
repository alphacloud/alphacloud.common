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
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Mvc;
    using Core.Data;


    static class ActionDiagnosticsHelper
    {
        public static Stack<T> GetStackFromContext<T>(ControllerContext controllerContext, string key)
        {
            var activityTraceStack = (Stack<T>) controllerContext.HttpContext.Items[key];
            if (activityTraceStack == null)
            {
                activityTraceStack = new Stack<T>();
                controllerContext.HttpContext.Items[key] = activityTraceStack;
            }
            return activityTraceStack;
        }


        public static string GetExecutingActionName(ActionExecutingContext filterContext)
        {
            var op = "/{0}/{1}".ApplyArgs(
                filterContext.Controller.GetType().Name.Replace("Controller", String.Empty),
                filterContext.ActionDescriptor.ActionName);
            return string.Intern(op);
        }


        public static StringBuilder GetActionMethodSignature(ActionExecutingContext filterContext)
        {
            var paramBuilder = new StringBuilder();
            foreach (var item in filterContext.ActionParameters)
            {
                if (paramBuilder.Length > 0)
                    paramBuilder.Append(", ");
                paramBuilder.AppendFormat("{0}:{1}", item.Key, item.Value ?? "<null>");
            }

            return paramBuilder;
        }


        public static string GetResultViewPath(ResultExecutedContext filterContext)
        {
            var result = filterContext.Result as ViewResultBase;
            if (result != null)
            {
                var view = result.View as RazorView;
                if (view != null) return "View({0})".ApplyArgs(view.ViewPath);
            }
            return filterContext.Result != null ? filterContext.Result.GetType().FullName : "<null>";
        }
    }
}