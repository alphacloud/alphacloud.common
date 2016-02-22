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
// ReSharper disable HeapView.BoxingAllocation
namespace Core.Tests.Data
{
    using Alphacloud.Common.Core.Data;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    [SetCulture("ru-RU")]
    public class NumberFormattingExtensionsTests
    {
        [Test]
        public void Double_AsStr_Should_UserNeutrlaCulture()
        {
            const double d = 1.01;
            d.AsStr().Should().Be("1.01");
        }


        [Test]
        public void Int_AsStr_Should_UserNeutralCulture()
        {
            const int i = 10000;
            i.AsStr().Should().Be("10000");
        }
    }
}