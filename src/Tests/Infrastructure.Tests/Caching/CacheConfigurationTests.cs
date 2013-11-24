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
            var cacheDuration = CacheDuration.LoadDurationSettings();
            cacheDuration.Tiny.Should().Be(1.Seconds());
            cacheDuration.Short.Should().Be(5.Seconds());
            cacheDuration.Balanced.Should().Be(30.Seconds());
            cacheDuration.Long.Should().Be(1.Minutes());
            cacheDuration.Huge.Should().Be(1.Hours());
        }
    }

    // ReSharper restore InconsistentNaming
}
