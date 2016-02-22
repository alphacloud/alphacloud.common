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
namespace Infrastructure.Tests.Castle
{
    using global::Castle.MicroKernel.Registration;
    using global::Castle.Windsor;
    using NUnit.Framework;

    [TestFixture]
    public class CastleWindsorTests
    {
        WindsorContainer _container;
        MyService _instance1;
        MyService _instance2;


        [SetUp]
        public void SetUp()
        {
            _container = new WindsorContainer();
            _instance1 = new MyService();
            _instance2 = new MyService();

            _container.Register(Component.For<IService>().Instance(_instance1).Named("service1"));
            _container.Register(Component.For<IService>().Instance(_instance2).Named("service2"));
        }


        [Test]
        public void ShouldResolveNamedInstances()
        {
            Assert.AreSame(_instance1, _container.Resolve<IService>("service1"), "service1");
            Assert.AreSame(_instance2, _container.Resolve<IService>("service2"), "service2");
        }


        [Test]
        public void ShouldResolveNamedInstancesUsingGenerics()
        {
            Assert.AreSame(_instance1, _container.Resolve<IService>("service1"), "service1");
            Assert.AreSame(_instance2, _container.Resolve<IService>("service2"), "service2");
        }

        #region Nested type: IService

        interface IService
        {
            void DoSomething(string arg);
        }

        #endregion

        #region Nested type: MyService

        class MyService : IService
        {
            #region IService Members

            public void DoSomething(string arg)
            {
            }

            #endregion
        }

        #endregion
    }
}