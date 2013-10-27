namespace Infrastructure.Tests.Caching
{
    using System;
    using System.Runtime.Caching;

    using Alphacloud.Common.Infrastructure.Caching;
    using Alphacloud.Common.Infrastructure.Caching.MemoryCache;

    using FluentAssertions;

    using JetBrains.Annotations;

    using NUnit.Framework;

    [TestFixture]
    public class MemoryCacheTests
    {
        private MemoryCacheAdapter _cache;
        private MemoryCache _memoryCache;

        [UsedImplicitly]
        [Serializable]
        private class Holder
        {
            public string Data { get; set; }
        }


        [SetUp]
        public void SetUp()
        {
            _memoryCache = new MemoryCache("test-cache");
            _cache = new MemoryCacheAdapter(_memoryCache, NullCacheHealthcheckMonitor.Instance,
                "test-cache");
        }


        [TearDown]
        public void TearDown()
        {
            _cache.Dispose();
        }


        [Test]
        public void CanPut()
        {
            _cache.Put("1111", "123", 5.Seconds());

            _memoryCache.Get("1111").Should().Be("123");
        }

        [Test]
        public void CanGet()
        {
            var key = Guid.NewGuid().ToString();
            _memoryCache.Set(key, "234", DateTimeOffset.UtcNow.AddMinutes(5));

            _cache.Get<string>(key).Should().Be("234");
        }

        [Test]
        public void CanRemove()
        {
            var key = Guid.NewGuid().ToString();
            _memoryCache.Set(key, "222", DateTimeOffset.UtcNow.AddSeconds(2));

            _cache.Remove(key);
            _memoryCache.Get(key).Should().BeNull();
        }
    }
}