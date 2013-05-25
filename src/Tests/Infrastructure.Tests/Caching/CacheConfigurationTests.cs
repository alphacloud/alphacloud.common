namespace Infrastructure.Tests.Caching
{
    using Alphacloud.Common.Infrastructure.Caching;

    using FluentAssertions;

    using NUnit.Framework;

    // ReSharper disable InconsistentNaming


    [TestFixture]
    class CacheConfigurationTests
    {
        [Test]
        public void ShouldReadDurationFromAppConfig()
        {
            CacheDuration.Instance.Short.Should().Be(5.Seconds());
            CacheDuration.Instance.Balanced.Should().Be(30.Seconds());
            CacheDuration.Instance.Long.Should().Be(1.Minutes());
            CacheDuration.Instance.Huge.Should().Be(1.Hours());
        }
    }

    // ReSharper restore InconsistentNaming
}
