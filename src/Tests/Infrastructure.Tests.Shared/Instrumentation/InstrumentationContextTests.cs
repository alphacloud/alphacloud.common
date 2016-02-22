#region copyright

// Copyright 2013-2016 Alphacloud.Net
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

// ReSharper disable ExceptionNotDocumented
// ReSharper disable HeapView.ClosureAllocation
// ReSharper disable ExceptionNotDocumentedOptional
// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable HeapView.ObjectAllocation
// ReSharper disable HeapView.ObjectAllocation.Evident
namespace Infrastructure.Tests.Instrumentation
{
    using System.Linq;
    using Alphacloud.Common.Core.Instrumentation;
    using Alphacloud.Common.Infrastructure.Instrumentation;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    class InstrumentationContextTests
    {
        InstrumentationContext _context;

        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _context = new InstrumentationContext();

            _context.AddCall("1", "1", 1.Seconds());
            _context.AddCall("2", "2", 2.Seconds());
            _context.AddCall("2", "3", 3.Seconds());
        }

        #endregion

        [Test]
        public void AddCall_Should_AddCall()
        {
            _context.AddCall(CallType.Database, "1", 2.Seconds());

            _context.GetCallCount(CallType.Database).Should().Be(1);
        }


        [Test]
        public void GetCallCount_NoFilterSpecified_Should_Return_TotalCallCount()
        {
            _context.GetCallCount().Should().Be(3);
        }


        [Test]
        public void GetCallCount_Should_FilterByType()
        {
            _context.GetCallCount("1").Should().Be(1);
            _context.GetCallCount("2").Should().Be(2);
        }


        [Test]
        public void GetCallDuration_NoFilterSpecified_Should_ReturnTotalDuration()
        {
            _context.GetCallDuration().Should().Be((1 + 2 + 3).Seconds());
        }


        [Test]
        public void GetCallDuration_Should_FilterByType()
        {
            _context.GetCallDuration("2").Should().Be(5.Seconds());
        }


        [Test]
        public void GetCallTypes_Should_ReturnAllCapturedCallTypes()
        {
            _context.GetCallTypes().Should().BeEquivalentTo(new object[] {"1", "2"});
        }


        [Test]
        public void GetDuplicatedCalls_Should_Return_DuplicatedCallsByType()
        {
            _context.AddCall("2", "3", 5.Seconds());
            _context.AddCall("1", "1", 10.Seconds());

            var dups = _context.GetDuplicatedCalls("2");
            dups.Should().HaveCount(1);
            var dup = dups.First();
            dup.CallCount.Should().Be(2);
            dup.CallType.Should().Be("2");
            dup.Operation.Should().Be("3");
            dup.Duration.Should().Be(8.Seconds());
        }
    }
}