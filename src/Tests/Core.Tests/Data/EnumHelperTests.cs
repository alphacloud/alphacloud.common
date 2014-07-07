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

namespace Core.Tests.Data
{
    using System;
    using System.Diagnostics;
    using Alphacloud.Common.Core.Data;
    using NUnit.Framework;


    [TestFixture]
    public class EnumHelperTests
    {
        enum LocalTest
        {
            [System.ComponentModel.Description("None")] None
        }


        [Test]
        [Category("Manual")]
        public void PerformanceTest()
        {
            // warm-up
            EnumHelper.GetDescription(LocalTest.None);
            EnumHelper.GetMemberDescription(typeof (LocalTest), LocalTest.None.ToString());

            const int iterationCount = 1000 * 1000;
            var ts = Stopwatch.StartNew();
            for (var i = 0; i < iterationCount; i++)
            {
                var d = EnumHelper.GetDescription(LocalTest.None);
            }
            Console.WriteLine(@"Elapsed time (cached): {0} ms", ts.ElapsedMilliseconds);

            ts = Stopwatch.StartNew();
            for (var i = 0; i < iterationCount; i++)
            {
                var d = EnumHelper.GetMemberDescription(typeof (LocalTest), LocalTest.None.ToString());
            }
            Console.WriteLine(@"Elapsed time (non-cached): {0} ms", ts.ElapsedMilliseconds);
        }
    }
}