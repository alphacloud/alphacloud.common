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

namespace Caching.Memcached.Tests
{
    using System;
    using Alphacloud.Common.Caching.Memcached;
    using Alphacloud.Common.Infrastructure.Caching;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture(Category = "Integration")]
    class MemcachedIntegrationTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _cache = _factory.GetCache();
        }

        #endregion

        MemcachedFactory _factory;
        ICache _cache;


        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            _factory = new MemcachedFactory();
            _factory.Initialize();
        }


        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _factory.Dispose();
            _factory = null;
        }


        [Test]
        public void CanGet()
        {
            var key = "get." + Guid.NewGuid();
            _cache.Put(key, "value", 10.Seconds());

            _cache.Get<string>(key)
                .Should().Be("value");
        }


        [Test]
        public void CanRemove()
        {
            var key = "remove." + Guid.NewGuid();
            _cache.Put(key, "Value", 10.Seconds());

            _cache.Get<string>(key)
                .Should().NotBeNull();

            _cache.Remove(key);
            _cache.Get(key)
                .Should().BeNull();
        }
    }
}