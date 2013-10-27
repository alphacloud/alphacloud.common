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

namespace Alphacloud.Common.Testing.Nunit.Attributes
{
    using System;
    using Core.Utils;
    using JetBrains.Annotations;
    using NUnit.Framework;

    /// <summary>
    ///   Set custom date time on <see cref="Clock" /> for time of the test execution.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    [PublicAPI]
    public sealed class SetClockAttribute : Attribute, ITestAction
    {
        readonly DateTime _datetime;
        readonly DateTimeOffset _datetimeOffset;
        readonly DateTime _datetimeUtc;


        /// <summary>
        ///   Initializes a new instance of the <see cref="SetClockAttribute" /> class.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="minute">The minute.</param>
        /// <param name="second">The second.</param>
        public SetClockAttribute(int year, int month, int day, int hour = 0, int minute = 0, int second = 0)
        {
            _datetime = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Local);
            _datetimeUtc = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
            _datetimeOffset = new DateTimeOffset(_datetimeUtc);
        }

        #region Implementation of ITestAction

        /// <summary>
        ///   Executed before each test is run
        /// </summary>
        /// <param name="testDetails">Provides details about the test that is going to be run.</param>
        public void BeforeTest(TestDetails testDetails)
        {
            Clock.CurrentTime = () => _datetime;
            Clock.CurrentTimeUtc = () => _datetimeUtc;
            Clock.CurrentTimeWithZone = () => _datetimeOffset;
        }


        /// <summary>
        ///   Executed after each test is run
        /// </summary>
        /// <param name="testDetails">Provides details about the test that has just been run.</param>
        public void AfterTest(TestDetails testDetails)
        {
            Clock.Reset();
        }


        /// <summary>
        ///   Provides the target for the action attribute
        /// </summary>
        /// <returns>The target for the action attribute</returns>
        public ActionTargets Targets
        {
            get { return ActionTargets.Test; }
        }

        #endregion
    }
}