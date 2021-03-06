﻿#region copyright

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

using Alphacloud.Common.Infrastructure.Caching;
using Alphacloud.Common.Testing.Nunit;
using Moq;

// ReSharper disable ExceptionNotDocumented
// ReSharper disable HeapView.ClosureAllocation
// ReSharper disable ExceptionNotDocumentedOptional
// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable HeapView.ObjectAllocation
// ReSharper disable HeapView.ObjectAllocation.Evident
namespace Infrastructure.Tests.Caching
{
    abstract class CompositeCacheTestsBase : MockedTestsBase
    {
        protected Mock<ICache> LocalCache { get; private set; }

        protected Mock<ICache> RemoteCache { get; private set; }


        protected override void DoSetup()
        {
            LocalCache = Mockery.Create<ICache>();
            LocalCache.SetupGet(lc => lc.Name).Returns("LocalCache");
            RemoteCache = Mockery.Create<ICache>();
        }


        protected override void DoTearDown()
        {
        }
    }
}