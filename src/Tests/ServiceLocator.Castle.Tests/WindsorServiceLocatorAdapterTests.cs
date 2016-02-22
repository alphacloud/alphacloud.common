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
// ReSharper disable HeapView.ObjectAllocation
namespace ServiceLocator.Castle.Tests
{
    using Alphacloud.Common.ServiceLocator.Castle;
    using FluentAssertions;
    using global::Castle.MicroKernel.Registration;
    using global::Castle.Windsor;
    using NUnit.Framework;

    [TestFixture]
    public class WindsorServiceLocatorAdapterTests
    {
        WindsorServiceLocatorAdapter _adapter;

        WindsorContainer _container;
        Service _service1;
        Service _service2;

        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _container = new WindsorContainer();
            _service1 = new Service("service 1");
            _service2 = new Service("service 2");

            _container.Register(Component.For<IService>().Instance(_service1).Named("service1"));
            _container.Register(Component.For<IService>().Instance(_service2).Named("service2"));
            _adapter = new WindsorServiceLocatorAdapter(_container);
        }

        #endregion

        [Test]
        public void ShouldResolveNamedInstancesWithGenericResolver()
        {
            _adapter.GetInstance<IService>("service1").Should().BeSameAs(_service1);
            _adapter.GetInstance<IService>("service2").Should().BeSameAs(_service2);
        }


        [Test]
        public void ShouldResolveNamedInstancesWithNonGenericResolver()
        {
            _adapter.GetInstance(typeof (IService), "service1")
                .Should().BeSameAs(_service1);
            _adapter.GetInstance(typeof (IService), "service2")
                .Should().BeSameAs(_service2);
        }
    }
}