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
    using System.Diagnostics;
    using System.Web.Mvc;
    using global::Common.Logging;
    using Infrastructure.Instrumentation;
    using JetBrains.Annotations;


    /// <summary>
    ///   Instrument Controller Action attribute.
    /// </summary>
    [PublicAPI]
    [BaseTypeRequired(typeof (IController))]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class InstrumentActionAttribute : ActionFilterAttribute
    {
        const string TimerKey = "lpl.common.instrumentation.actionTimer";
        const string ActionNameKey = "lpl.common.instrumentation.actionName";
        static readonly ILog s_log = LogManager.GetCurrentClassLogger();


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;
            if (InstrumentationRuntime.Instance.GetConfiguration().Enabled)
            {
                filterContext.RequestContext.HttpContext.Items[TimerKey] = Stopwatch.StartNew();
                filterContext.RequestContext.HttpContext.Items[ActionNameKey] =
                    ActionDiagnosticsHelper.GetExecutingActionName(filterContext);
            }
        }


        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.IsChildAction || !InstrumentationRuntime.Instance.GetConfiguration().Enabled)
                return;
            var sw = filterContext.RequestContext.HttpContext.Items[TimerKey] as Stopwatch;
            filterContext.RequestContext.HttpContext.Items.Remove(TimerKey);
            var actionName = filterContext.RequestContext.HttpContext.Items[ActionNameKey] as string;

            if (sw == null || actionName == null)
                s_log.Debug("Stopwatch or ActionName not found in HttpContext");
            else
                InstrumentationRuntime.Instance.OnOperationCompleted(this, actionName, sw.Elapsed);
        }
    }
}