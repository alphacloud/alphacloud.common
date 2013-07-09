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
    using Enyim.Caching;
    using Enyim.Caching.Memcached;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    class MemcachedAdapterTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _mockery = new MockRepository(MockBehavior.Default);
            _clientMock = _mockery.Create<IMemcachedClient>();
            _healthCheck = _mockery.Create<ICacheHealthcheckMonitor>();
            _healthCheck.SetupGet(hc => hc.IsCacheAvailable).Returns(true);
            _adapter = new MemcachedAdapter(_clientMock.Object, _healthCheck.Object, null);
        }


        [TearDown]
        public void TearDown()
        {
            _adapter = null;
            _mockery.Verify();
        }

        #endregion

        Mock<IMemcachedClient> _clientMock;
        MockRepository _mockery;
        Mock<ICacheHealthcheckMonitor> _healthCheck;
        MemcachedAdapter _adapter;


        [Test]
        public void Clear_ClearCache()
        {
            _adapter.Clear();

            _clientMock.Verify(c => c.FlushAll());
        }


        [Test]
        public void Get_Should_DelegateToClient()
        {
            _clientMock.Setup(c => c.Get("key"))
                .Returns("value");

            _adapter.Get("key")
                .Should().Be("value");
        }


        [Test]
        public void Put_Should_DelegateToClient()
        {
            _clientMock.Setup(
                cli => cli.Store(StoreMode.Set, It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()))
                .Returns(true);
            _adapter.Put("key-d", "value", 3.Seconds());

            _clientMock.Verify(c => c.Store(StoreMode.Set, "key-d", "value", 3.Seconds()));
        }
    }
}