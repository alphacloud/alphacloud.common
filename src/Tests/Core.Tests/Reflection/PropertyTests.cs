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
        [SetUp]
        public void SetUp()
        {
            _obj = new Aggregate {NestedObj = new Nested()};
        }

        private Aggregate _obj;

        private class Nested
        {
            public string Name { get; set; }
        }

        private class Aggregate
        {
            public int Id { get; set; }
            public Nested NestedObj { get; set; }
            public List<int> List { get; set; }
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
    }
}