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
namespace Infrastructure.Tests.Caching
{
    using System;
    using Alphacloud.Common.Infrastructure.Caching;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class AdaptiveTimeoutTests
    {
        ProportionalTimeout _timeout;
        TimeSpan _minTtl;
        TimeSpan _maxTtl;


        [SetUp]
         public void SetUp()
        {
            _minTtl = 10.Seconds();
            _maxTtl = 20.Seconds();
            _timeout = new ProportionalTimeout(_minTtl, _maxTtl, 10);
        }


        [Test]
        public void When_ZeroTimeoutIsSpecifued_Should_ReturnMinimalTtl()
        {
            _timeout.GetLocalTimeout(TimeSpan.Zero)
                .Should().Be(_minTtl);
        }
 
        [Test]
        public void When_CalculatedTimeoutIsBelowMinTtl_Should_ReturnMinimalTtl()
        {
            _timeout.GetLocalTimeout(99.Seconds())
                .Should().Be(_minTtl);
        }

        [Test]
        public void When_CalculatedTimeouIsAboveMaximumTtl_Should_ReturnMaximumTtl()
        {
            _timeout.GetLocalTimeout(210.Seconds())
                .Should().Be(_maxTtl);
        }

        [Test]
        public void When_CalculatedTimeoutIsInAllowedRange_Should_ReturnPercentage()
        {
            _timeout.GetLocalTimeout(120.Seconds())
                .Should().Be(12.Seconds());
        }
    }
}