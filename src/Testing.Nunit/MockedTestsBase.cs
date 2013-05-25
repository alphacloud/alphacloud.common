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

namespace Testing.Nunit
{
    using Moq;

    using NUnit.Framework;

    //// ReSharper disable InconsistentNaming

    /// <summary>
    ///   Base class for tests with mocks.
    /// </summary>
    [TestFixture]
    public abstract class MockedTestsBase
    {
        /// <summary>
        ///   Setup tests
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _mockery = new MockRepository(MockBehavior.Default);
            DoSetup();
        }

        /// <summary>
        ///   Cleanup tests
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            DoTearDown();
            Mockery.Verify();
        }

        MockRepository _mockery;

        protected MockRepository Mockery
        {
            get { return _mockery; }
        }

        protected abstract void DoSetup();
        protected abstract void DoTearDown();
    }

//// ReSharper restore InconsistentNaming
}
