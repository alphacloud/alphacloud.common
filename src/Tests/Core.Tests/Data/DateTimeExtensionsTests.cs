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

// ReSharper disable ExceptionNotDocumented
// ReSharper disable HeapView.ClosureAllocation
// ReSharper disable ExceptionNotDocumentedOptional
// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable HeapView.ObjectAllocation
// ReSharper disable HeapView.ObjectAllocation.Evident
namespace Core.Tests.Data
{
    using System;

    using Alphacloud.Common.Core.Data;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    class DateTimeExtensionsTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _date1 = new DateTime(2013, 10, 18, 22, 33, 56);
        }

        #endregion

        DateTime _date1;


        [Test]
        public void RoundMinutes_Should_RoundMinutesToClosestValue()
        {
            _date1.RoundMinutes(10).Should().Be(new DateTime(2013, 10, 18, 22, 30, 00));
        }
    }

}
