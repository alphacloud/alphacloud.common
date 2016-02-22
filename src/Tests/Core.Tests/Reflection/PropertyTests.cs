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
namespace Core.Tests.Reflection
{
    using System;
    using System.Collections.Generic;
    using Alphacloud.Common.Core.Reflection;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class PropertyTests
    {
        Aggregate _obj;


        [SetUp]
        public void SetUp()
        {
            _obj = new Aggregate {NestedObj = new Nested()};
        }


        [Test]
        public void Name_Should_HandleNastedProperties()
        {
            Property.Name<Aggregate>(a => a.NestedObj.Name)
                .Should().Be("NestedObj.Name");
        }


        [Test]
        public void Name_Should_HandleNestedPropertiesFromInstance()
        {
            Property.Name(() => _obj.NestedObj.Name, false)
                .Should().Be("_obj.NestedObj.Name");
        }


        [Test]
        public void Name_Should_Throw_IfIndexerSpecified()
        {
            Assert.Throws<InvalidOperationException>(() => Property.Name<Aggregate>(a => a.List[0]));
        }


        [Test]
        public void Name_Should_ReturnPropertyName()
        {
            Property.Name<Aggregate>(a => a.Id)
                .Should().Be("Id");
        }


        [Test]
        public void Name_Should_ReturnPropertyNameFromInstance()
        {
            Property.Name(() => _obj.Id)
                .Should().Be("Id");
        }


        [Test]
        public void PropertyName_Should_HandleNestedProperties()
        {
            _obj.PropertyName(o => o.NestedObj.Name)
                .Should().Be("NestedObj.Name");
        }


        [Test]
        public void PropertyName_Should_ReturnPropertyName()
        {
            _obj.PropertyName(o => o.Id)
                .Should().Be("Id");
        }

        #region Nested type: Aggregate

        class Aggregate
        {
            public int Id { get; set; }
            public Nested NestedObj { get; set; }
            public List<int> List { get; set; }
        }

        #endregion

        #region Nested type: Nested

        class Nested
        {
            public string Name { get; set; }
        }

        #endregion
    }
}