namespace Infrastructure.Tests.Caching
{
    using System.Collections.Specialized;

    using Alphacloud.Common.Infrastructure.Caching;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    internal class DisabledCompositeFactoryTests
    {
        private CompositeCacheFactory _factory;


        [SetUp]
        public void SetUp()
        {
            var parameters = new NameValueCollection {{"enabled", "false"}};
            _factory = new CompositeCacheFactory(parameters);
            _factory.Initialize();
        }

        [Test]
        public void CreateCache_Should_CreateNullCache()
        {
            _factory.GetCache()
                .Should().BeOfType<NullCache>();
        }
    }
}