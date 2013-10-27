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
    using Alphacloud.Common.Caching.Memcached.CommonLogging;
    using Enyim.Caching;
    using Enyim.Caching.Memcached;
    using FluentAssertions;
    using NUnit.Framework;
    using ILog = Common.Logging.ILog;

    [TestFixture]
    public class MemcachedClientTests
    {
        ILog s_log = Common.Logging.LogManager.GetCurrentClassLogger();

        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _client = new MemcachedClient();
        }


        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
        }

        #endregion

        MemcachedClient _client;


        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            LogManager.AssignFactory(new MemcachedLogFactory());
        }


        [Test]
        public void MultiGet()
        {
            s_log.Debug("MultiGet");
            var k1 = Guid.NewGuid().ToString();
            var k2 = Guid.NewGuid().ToString();


            _client.Store(StoreMode.Set, k1, "1", 5.Minutes());
            _client.Store(StoreMode.Set, k2, "2", 5.Minutes());

            _client.Get(k1).Should().NotBeNull();

            var res = _client.Get(new[] {k1, k2});
            res.Should().ContainKey(k1).And.Subject[k1].Should().Be("1");
            res.Should().ContainKey(k2).And.Subject[k2].Should().Be("2");
        }
    }
}