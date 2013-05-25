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
    using Alphacloud.Common.Infrastructure.Caching;

    using FluentAssertions;

    using Moq;

    using NUnit.Framework;

    //// ReSharper disable InconsistentNaming

    [TestFixture]
    class CompositeCacheTests : CompositeCacheTestsBase
    {
        const string Key = "key";
        const string Value = "val";
        CompositeCache m_cache;

        protected override void DoSetup()
        {
            base.DoSetup();
            m_cache = new CompositeCache(LocalCache.Object, RemoteCache.Object, 2.Seconds());
        }

        [Test]
        public void Get_FromRemoteCache_Should_UpdateLocal()
        {
            RemoteCache.Setup(c => c.Get(Key))
                .Returns(Value);

            m_cache.Get(Key).Should().Be(Value, "data lost");

            LocalCache.Verify(c => c.Put(Key, Value, 2.Seconds()), "should update local cache");
        }

        [Test]
        public void Get_Should_ReturnDataFromLocaCache()
        {
            LocalCache.Setup(c => c.Get(Key))
                .Returns(Value);

            m_cache.Get(Key)
                .Should().Be(Value, "wrong data returned from cache");
            RemoteCache.Verify(c => c.Get(Key), Times.Never(),
                "should not access remote cache if local cache contains data");
        }

        [Test]
        public void Put_Should_UpdateCaches()
        {
            m_cache.Put(Key, Value, 30.Seconds());

            LocalCache.Verify(c => c.Put(Key, Value, 2.Seconds()), "should update local cache with LocalTimeout");
            RemoteCache.Verify(c => c.Put(Key, Value, 30.Seconds()), "should update remote cache");
        }

        [Test]
        public void Remove_Should_RemoveDataFromCaches()
        {
            m_cache.Remove(Key);

            LocalCache.Verify(c => c.Remove(Key), "should remove from local cache");
            RemoteCache.Verify(c => c.Remove(Key), "should remove from remote cache");
        }
    }


    // ReSharper restore InconsistentNaming
}
