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
    using Alphacloud.Common.Core.Reflection;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    class ReflectionExtensionsTests
    {
        [SetUp]
        public void SetUp()
        {
        }


        [TearDown]
        public void TearDown()
        {
        }


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
        }


        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
        }


        [Test]
        public void ToDataDictionary_ShouldEnumaratAllPublicProperties()
        {
            var data = new {Prop1 = "string", Prop2 = 22};
            var dic = data.ToDataDictionary();
            dic.Count.Should().Be(2);
            dic.Keys.Should().Contain("Prop1").And.Contain("Prop2");
        }


        [Test]
        public void ToDataDictionary_ShouldSkipIndexedProperties()
        {
            new IndexedData().ToDataDictionary().Should().HaveCount(0, "should skip indexed properties");
        }


        [Test]
        public void ToDataDictionary_ShouldSkipNonPublicProperties()
        {
            new Test1().ToDataDictionary()
                .Should().ContainKey("PublicProp")
                .And.ContainValue("public")
                .And.HaveCount(1, "non-public properties should not be listed");
        }


        [Test]
        public void PropertyValue_Should_ReturnPropertyValue()
        {
            new Test1().PropertyValue<string>("NonPublic")
                .Should().Be("non-public");
        }


        [Test]
        public void FieldValue_Should_ReturnFieldValue()
        {
            new Test1().FieldValue<string>("_field")
                .Should().Be("private-field");
        }

        #region Nested type: IndexedData

        class IndexedData
        {
            public string this[string key]
            {
                get { return key; }
            }
        }

        #endregion

        #region Nested type: Test1

        class Test1
        {
#pragma warning disable 169
            string _field = "private-field";
#pragma warning restore 169

            #region .ctor

            public Test1()
            {
                PublicProp = "public";
                NonPublic = "non-public";
            }

            #endregion

            public string PublicProp { get; set; }
            protected string NonPublic { get; set; }
        }

        #endregion
    }
}