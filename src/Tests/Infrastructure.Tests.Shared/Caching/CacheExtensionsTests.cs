#region copyright

// Copyright 2013 Alphacloud.Net
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

namespace Infrastructure.Tests.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alphacloud.Common.Infrastructure.Caching;
    using Alphacloud.Common.Testing.Nunit;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    class CacheExtensionsTests : MockedTestsBase
    {
        Mock<ICache> _cache;
        string[] _keys;


        protected override void DoSetup()
        {
            _cache = Mockery.Create<ICache>();
            _keys = new[] {
                "k1", "k2", "k3"
            };
        }


        [Test]
        public void GetOrLoad_ShouldNot_UpdateCacheIfValidationFailed()
        {
            var res = _cache.Object.GetOrLoad("key", () => new Cached<string>("value"), 10.Seconds(), cached => false);

            _cache.Verify(cache => cache.Put("key", It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never(),
                "should not update cache if validation callback returns false");

            res.Item.Should().Be("value");
        }


        [Test]
        public void GetOrLoad_Should_CheckCacheFirst()
        {
            _cache.Setup(cache => cache.Get("1")).Returns("cached");

            _cache.Object.GetOrLoad("1", () => "loaded", 10.Seconds())
                .Should().Be("cached");
        }


        [Test]
        public void GetOrLoad_Should_UpdateCache()
        {
            _cache.Object.GetOrLoad("key", () => "value", 10.Seconds())
                .Should().Be("value");

            _cache.Verify(cache => cache.Put("key", "value", 10.Seconds()), "cache was not updated");
        }


        [Test]
        public void GetOrLoad_Should_UpdateCacheIfValudationSucceed()
        {
            var cachedValue = new Cached<string>("value");
            _cache.Object.GetOrLoad("key", () => cachedValue, 10.Seconds(), cached => true);
            _cache.Verify(cache => cache.Put("key", cachedValue, 10.Seconds()), Times.Once(),
                "should update cache if validation succeed");
        }


        [Test]
        public void MultiGetOrLoad_Should_Query_Cache()
        {
            _cache.Setup(c => c.Get(Argument.IsCollection(new[] {"k1", "k2"})))
                .Returns(new Dictionary<string, object> {{"k1", "v1"}, {"k2", "v2"}});

            var res = _cache.Object.GetOrLoad(_keys, keys => new Dictionary<string, object>(), 1.Minutes());

            res.Should().Contain(new KeyValuePair<string, object>("k1", "v1"))
                .And.Contain(new KeyValuePair<string, object>("k2", "v2"));
        }


        [Test]
        public void MultiGetOrLoad_Should_LoadMissingData()
        {
            _cache.Setup(c => c.Get(Argument.IsCollection(new[] { "k1", "k2" })))
                .Returns(new Dictionary<string, object> { { "k1", "v1" }, { "k2", "v2" } });

            var res = _cache.Object.GetOrLoad(_keys, keys => keys.ToDictionary<string, string, object>(k=>k, v=>v), 1.Minutes());

            res.Should().Contain(new KeyValuePair<string, object>("k3", "k3"), "external data was not loaded");
            res.Should().ContainKeys("k1", "k2");
            res.Should().ContainValues("v1", "v2");
        }


        [Test]
        public void MultiGetOrLoad_Should_UpdateCacheWithLoadedData()
        {
            _cache.Setup(c => c.Get(Argument.IsCollection(new[] { "k1" })))
                .Returns(new Dictionary<string, object> { { "k1", "v1" }});

            _cache.Object.GetOrLoad(_keys, keys => keys.ToDictionary<string, string, object>(k => k, v => v), 1.Minutes());

            var loadedData = new Dictionary<string, object> {
                {"k2", "k2"},
                {"k3", "k3"}
            };

            _cache.Verify(c => c.Put(Argument.IsDictionary(loadedData), 1.Minutes()), "cache was not updated");

        }
    }
}