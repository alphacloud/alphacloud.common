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

namespace Infrastructure.Tests.Instrumentation
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Common.Logging;
    using Logging;
    using NUnit.Framework;

    [TestFixture]
    class CorrelationManagerTests : Log4NetTestsBase
    {
        static readonly ILog s_log = LogManager.GetCurrentClassLogger();


        [Test]
        public void ShouldPass_ActivityId_To_ChildThread()
        {
            Trace.CorrelationManager.StartLogicalOperation("Load");

            Trace.CorrelationManager.ActivityId = Guid.NewGuid();

            LogCorrelation("");

            var t = new Task(() => LogCorrelation(""));
            t.Start();
            t.Wait();

            Trace.CorrelationManager.StopLogicalOperation();
            s_log.InfoFormat("ActivityID after logical operation: {0}", Trace.CorrelationManager.ActivityId);
        }


        static void LogCorrelation(string msg)
        {
            s_log.InfoFormat("{0} ActivityId: {1}, Stack{2}: ", Trace.CorrelationManager.ActivityId);
        }
    }
}