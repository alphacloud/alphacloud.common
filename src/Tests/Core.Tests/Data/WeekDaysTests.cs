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

namespace Core.Tests.Data
{
    using Alphacloud.Common.Core.Data;

    using FluentAssertions;

    using NUnit.Framework;

//// ReSharper disable InconsistentNaming

    [TestFixture]
    class WeekDaysTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
        }


        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
        }


        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
        }


        [Test]
        public void Can_InitializeFromProperties()
        {
            var week = new WeekDays {
                Monday = true,
                Tuesday = true,
                Wednesday = true,
                Thursday = true,
                Friday = true,
                Saturday = true,
                Sunday = true
            };
            week.Pack().Should().Be(Week.WholeWeek);
        }


        [Test]
        public void Equals_Should_CompareByWeekDays()
        {
            var week1 = new WeekDays(Week.WholeWeek);
            var week2 =
                new WeekDays(Week.Monday | Week.Tuesday | Week.Wednesday | Week.Thursday | Week.Friday | Week.Saturday |
                             Week.Sunday);

            week1.Equals(week2).Should().BeTrue();
        }


        [Test]
        public void IsEmpty_Should_CheckWeekDays()
        {
            new WeekDays(Week.None).IsEmpty().Should().BeTrue();
            new WeekDays(Week.WholeWeek).IsEmpty().Should().BeFalse();
            new WeekDays(Week.Wednesday).IsEmpty().Should().BeFalse();
        }


        [Test]
        public void NewWeek_Should_BeEmpty()
        {
            new WeekDays().IsEmpty().Should().BeTrue();
        }


        [Test]
        public void Pack_Should_ConvertToEnum()
        {
            new WeekDays(Week.Monday | Week.Sunday).Pack()
                .Should().Be(Week.Monday | Week.Sunday);
        }
    }

//// ReSharper restore InconsistentNaming
}
