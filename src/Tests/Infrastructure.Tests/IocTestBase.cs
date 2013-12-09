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

namespace Infrastructure.Tests
{
    using Alphacloud.Common.ServiceLocator.Castle;
    using global::Castle.Windsor;
    using Microsoft.Practices.ServiceLocation;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class IocTestBase
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Mockery = new MockRepository(MockBehavior.Default);
            Container = new WindsorContainer();
            var adapter = new WindsorServiceLocatorAdapter(Container);
            ServiceLocator.SetLocatorProvider(() => adapter);

            DoSetup();
        }


        [TearDown]
        public void TearDown()
        {
            DoTeardown();
            
            Container.Dispose();
            ServiceLocator.SetLocatorProvider(null);
            Mockery.Verify();
        }

        #endregion

        protected MockRepository Mockery { get; private set; }

        protected IWindsorContainer Container { get; private set; }


        protected virtual void DoSetup()
        {
        }


        protected virtual void DoTeardown()
        {
        }
    }
}