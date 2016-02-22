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

// ReSharper disable ExceptionNotDocumented
// ReSharper disable HeapView.ClosureAllocation
// ReSharper disable ExceptionNotDocumentedOptional
// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable HeapView.ObjectAllocation
// ReSharper disable HeapView.ObjectAllocation.Evident
namespace Infrastructure.Tests.Caching
{
    using System;

    using Alphacloud.Common.Core.Data;
    using Alphacloud.Common.Infrastructure.Caching;

    using FluentAssertions;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    class CompositeCacheDebugModeTests : CompositeCacheTestsBase
    {
        const string Key = "key";
        CompositeCache _cache;
        string _fullKey;


        protected override void DoSetup()
        {
            base.DoSetup();
            _cache = new CompositeCache(LocalCache.Object, RemoteCache.Object, new FixedTimeoutStrategy(2.Seconds()), true);
            _fullKey = "{0}{1}".ApplyArgs(_cache.GetCacheKeyPrefix(), Key);
        }


        [Test]
        public void CachePrefix_Should_ContainMachineName()
        {
            _cache.GetCacheKeyPrefix()
                .Should().StartWith(Environment.MachineName);
        }


        [Test]
        public void Get_Should_PrefixKeyWithMachineName()
        {
            _cache.Get(Key);

            LocalCache.Verify(c => c.Get(_fullKey));
            RemoteCache.Verify(c => c.Get(_fullKey));
        }


        [Test]
        public void Put_Should_PrefixKeyWithMachineName()
        {
            _cache.Put(Key, "value", 30.Seconds());

            LocalCache.Verify(c => c.Put(_fullKey, "value", It.IsAny<TimeSpan>()));
            RemoteCache.Verify(c => c.Put(_fullKey, "value", 30.Seconds()));
        }


        [Test]
        public void Remove_Should_PrefixKeyWithMachineName()
        {
            _cache.Remove(Key);

            LocalCache.Verify(c => c.Remove(_fullKey));
            RemoteCache.Verify(c => c.Remove(_fullKey));
        }
    }
}
