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
        MemoryCacheAdapter _cache;
        MemoryCache _memoryCache;


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

        #region Nested type: Holder

        [UsedImplicitly]
        [Serializable]
        class Holder
        {
            public string Data { get; set; }
        }

        #endregion
    }
}