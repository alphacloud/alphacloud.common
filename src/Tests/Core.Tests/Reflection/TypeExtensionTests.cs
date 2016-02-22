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
// ReSharper disable HeapView.BoxingAllocation
namespace Core.Tests.Reflection
{
    using System.Linq;
    using Alphacloud.Common.Core.Reflection;
    using FluentAssertions;
    using NUnit.Framework;
    using TypeExtensions = Alphacloud.Common.Core.Reflection.TypeExtensions;

    [TestFixture]
    class TypeExtensionTests
    {
        [Test]
        public void IsOpenGenertiTypeImplementation_Should_Detect_BaseInterface()
        {
            typeof (Bar).ImplementsGeneric(typeof (IGenericInterface<>))
                .Should().BeTrue();
        }


        [Test]
        public void Implements_Should_DetectOpenGenericDefitinions()
        {
            typeof (ClosedType).Implements<IClosedInterface>()
                .Should().BeTrue();

            typeof (Foo).Implements(typeof (IGenericInterface<>));
        }


        [Test]
        public void Implements_Should_DetectGenericDefinition()
        {
            typeof (Bar).Implements<IGenericInterface<int>>()
                .Should().BeTrue();
        }


        [Test]
        public void Implements_Should_DetectClassImplementation()
        {
            typeof (Foo).Implements<Bar>()
                .Should().BeTrue();
        }


        [Test]
        public void FindNonGenericInterfaceOf_Should_FindAllNonGenericInterfacesImplementedByType()
        {
            // ReSharper disable once InvokeAsExtensionMethod
            var types =
                TypeExtensions.FindNonGenericInterfacesOf(typeof (ClosedType), typeof (IGenericInterface<>)).ToArray();
            types.Should().BeEquivalentTo(typeof (IClosedInterface));

            typeof (ClosedType).FindNonGenericInterfacesOf(typeof (IGenericInterface<>))
                .Should().BeEquivalentTo(typeof (IClosedInterface));
        }

        #region Nested type: Bar

        class Bar : IGenericInterface<int>
        {
            #region IGenericInterface<int> Members

            public int Get()
            {
                return 0;
            }

            #endregion
        }

        #endregion

        #region Nested type: ClosedType

        class ClosedType : IClosedInterface
        {
            #region IClosedInterface Members

            public int Get()
            {
                return 1;
            }

            #endregion
        }

        #endregion

        #region Nested type: Foo

        class Foo : Bar
        {
        }

        #endregion

        #region Nested type: IClosedInterface

        interface IClosedInterface : IGenericInterface<int>
        {
        }

        #endregion

        #region Nested type: IGenericInterface

        interface IGenericInterface<out T>
        {
            T Get();
        }

        #endregion
    }
}