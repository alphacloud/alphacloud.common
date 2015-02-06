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
    using System.Web;
    using System.Web.Mvc;
    using Core.Instrumentation;
    using global::Common.Logging;
    using Infrastructure.Instrumentation;
    using JetBrains.Annotations;


    /// <summary>
    ///   Initialize correlation Id for each request.
    /// </summary>
    [PublicAPI]
    [BaseTypeRequired(typeof (IController))]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class CorrelationIdAttribute : ActionFilterAttribute
    {
        const string ActivityTraceKey = "Alphacloud.ActvityTrace.ActivityTraceStack";
        const string ChildActionStackKey = "Alphacloud.ActvityTrace.ChildActionStack";

        static readonly ILog s_log = LogManager.GetLogger(InstrumentationConstants.ActivityTraceLogger);

        /// <summary>
        ///   Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext"> The filter context. </param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var executingActionName = ActionDiagnosticsHelper.GetExecutingActionName(filterContext);
            if (!filterContext.IsChildAction)
            {
                string correlationid = CorrelationIdGenerator.NewId();
                var diagnosticContext = InstrumentationRuntime.Instance.GetDiagnosticContext();
                diagnosticContext.Clear();
                diagnosticContext.Push(correlationid);

                s_log.Debug(m => m("Executing Action '{0}', params: {1}", 
                    executingActionName, ActionDiagnosticsHelper.GetActionMethodSignature(filterContext)));
            }
            else
            {
                s_log.Debug(m => m("Executing ChildAction {0} ({1})",
                    executingActionName, ActionDiagnosticsHelper.GetActionMethodSignature(filterContext)));

                var actionStack = ActionDiagnosticsHelper.GetStackFromContext<string>(filterContext, ChildActionStackKey);
                actionStack.Push(executingActionName);
            }
            base.OnActionExecuting(filterContext);
        }


        /// <summary>
        ///   Called by the ASP.NET MVC framework after the action result executes.
        /// </summary>
        /// <param name="filterContext"> The filter context. </param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);

            s_log.Debug(m => m("Executed result: {0}", ActionDiagnosticsHelper.GetResultViewPath(filterContext) ?? "?"));

            if (!filterContext.IsChildAction)
            {
                InstrumentationRuntime.Instance.GetDiagnosticContext().Clear();
            }
            else
            {
                // log child action completion
                var actionStack = (Stack<string>) filterContext.HttpContext.Items[ChildActionStackKey];
                if (actionStack != null && actionStack.Count > 0)
                {
                    string actionName = actionStack.Pop();
                    s_log.DebugFormat("Executed child action {0}", actionName);
                }
                else
                    s_log.Warn("Child action stack underflow");
            }
        }


        /// <summary>
        ///   Clean orphan correlation ids.
        /// </summary>
        /// <param name="httpContext"></param>
        public static void CleanupIdStack(HttpContext httpContext)
        {
            var activityTraceStack = (Stack<IDisposable>) httpContext.Items[ActivityTraceKey];
            if (activityTraceStack == null || activityTraceStack.Count == 0)
                return;

            foreach (var disposable in activityTraceStack)
            {
                disposable.Dispose();
            }
            s_log.DebugFormat("Removed {0} orphan IDs", activityTraceStack.Count);
            activityTraceStack.Clear();
        }
    }
}