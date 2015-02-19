#region copyright

// Copyright 2013-2015 Alphacloud.Net
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

namespace Core.Tests.Utils
{
    using System.Collections.Generic;
    using Alphacloud.Common.Core.Data;
    using Alphacloud.Common.Core.Utils;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class ObjectPoolTests
    {
        ObjectPool<string> _pool;


        [SetUp]
        public void SetUp()
        {
            _pool = new ObjectPool<string>(10,
                () => "pooled at {0:yyyy/MM/dd hh:mm:ss.fff}".ApplyArgs(Clock.CurrentTime()));
        }


        [TearDown]
        public void TearDown()
        {
            _pool = null;
        }


        [Test]
        public void ShouldMaintainMaxPoolSize()
        {
            var items = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                items.Add(_pool.GetObject());
            }
            foreach (var item in items)
            {
                _pool.ReturnObject(item);
            }

            _pool.Count.Should().Be(10);
        }


        [Test]
        public void PooledObjectWrapperShouldReturnObjectToPoolOnDisposal()
        {
            using (_pool.GetWrappedObject())
            {
                _pool.Count.Should().Be(0);
            }
            _pool.Count.Should().Be(1, "Should return object to pool");
        }
    }
}