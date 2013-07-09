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
    using System.Collections.Generic;
    using System.Net;
    using System.Reflection;
    using Alphacloud.Common.Caching.Memcached;
    using Alphacloud.Common.Infrastructure.Caching;
    using Enyim.Caching.Memcached;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class MemcachedStatsParserTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _stats = CreateStatsInstance(CreateStats());
            _cacheStatistics = new MemcachedStatsParser(_stats).GetStatistics();
        }

        #endregion

        ServerStats _stats;
        CacheStatistics _cacheStatistics;


        static ServerStats CreateStatsInstance(Dictionary<IPEndPoint, Dictionary<string, string>> stats)
        {
            // ServerStats has no public constructor, so using reflection to instantiate it.
            var ctor =
                typeof (ServerStats).GetConstructor(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,
                    new[] {typeof (Dictionary<IPEndPoint, Dictionary<string, string>>)},
                    null);
            var obj = ctor.Invoke(new[] {(object) stats});
            return (ServerStats) obj;
        }


        static Dictionary<IPEndPoint, Dictionary<string, string>> CreateStats()
        {
            return new Dictionary<IPEndPoint, Dictionary<string, string>>
            {
                {
                    new IPEndPoint(0x0100007F, 11211), new Dictionary<string, string>
                    {
                        {"uptime", "182"},
                        {"cmd_get", "200"},
                        {"get_hits", "100"},
                        {"cmd_set", "300"},
                        {"version", "1.0.0"},
                        {"curr_items", "1000"},
                    }
                },
                {
                    new IPEndPoint(0x0200007F, 11211), new Dictionary<string, string>
                    {
                        {"uptime", "178"},
                        {"cmd_get", "100"},
                        {"get_hits", "50"},
                        {"cmd_set", "300"},
                        {"version", "1.0.0"},
                        {"curr_items", "300"},
                    }
                }
            };
        }


        [Test]
        public void ShouldCalculateHitRate()
        {
            _cacheStatistics.HitRate.Should().Be(50);
        }


        [Test]
        public void ShouldCalculateItemCount()
        {
            _cacheStatistics.ItemCount.Should().Be(1300);
        }


        [Test]
        public void ShouldParseGetCount()
        {
            _cacheStatistics.GetCount.Should().Be(300);
        }


        [Test]
        public void ShouldParseHitCount()
        {
            _cacheStatistics.HitCount.Should().Be(150);
        }


        [Test]
        public void ShouldParsePutCount()
        {
            _cacheStatistics.PutCount.Should().Be(600);
        }


        [Test]
        public void ShouldParseServerStatistics()
        {
            _cacheStatistics.Nodes.Should().HaveCount(2, "wrong server count");

            var s1 = _cacheStatistics.Nodes[0];
            s1.Server.Should().Be("127.0.0.1:11211");
            s1.HitCount.Should().Be(100);
            s1.GetCount.Should().Be(200);
            s1.PutCount.Should().Be(300);
            s1.ItemCount.Should().Be(1000);
        }
    }
}