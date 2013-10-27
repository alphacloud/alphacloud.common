namespace Core.Tests.Reflection
{
    using Alphacloud.Common.Core.Reflection;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    internal class ReflectionExtensionsTests
    {
        [SetUp]
        public void SetUp()
        {
        }


        [TearDown]
        public void TearDown()
        {
        }

        private class IndexedData
        {
            public string this[string key]
            {
                get { return key; }
            }
        }


        private class Test1
        {
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


        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
        }


        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
        }

        [Test]
        public void ShouldEnumaratAllPublicProperties()
        {
            var data = new {Prop1 = "string", Prop2 = 22};
            var dic = data.ToDataDictionary();
            dic.Count.Should().Be(2);
            dic.Keys.Should().Contain("Prop1").And.Contain("Prop2");
        }


        [Test]
        public void ShouldSkipIndexedProperties()
        {
            new IndexedData().ToDataDictionary().Should().HaveCount(0, "should skip indexed properties");
        }


        [Test]
        public void ShouldSkipNonPublicProperties()
        {
            new Test1().ToDataDictionary()
                .Should().ContainKey("PublicProp")
                .And.ContainValue("public")
                .And.HaveCount(1, "non-public properties should not be listed");
        }
    }
}