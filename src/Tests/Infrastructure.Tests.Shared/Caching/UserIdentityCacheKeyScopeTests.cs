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
    using Alphacloud.Common.Infrastructure.Caching;
    using Alphacloud.Common.Testing.Nunit.Attributes;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    class UserIdentityCacheKeyScopeTests
    {
        UserIdentityCacheKeyScope _scope;

        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _scope = new UserIdentityCacheKeyScope();
        }

        #endregion

        [Test]
        [SetPrincipal("identity", Roles = new[] {"1", "2"})]
        public void ShouldPrefixKeyWithCurrentThreadIdentity()
        {
            _scope.Encode("key")
                .Should().Be("identity.key");
        }
    }
}