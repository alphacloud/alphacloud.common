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
            typeof (Bar).ImplementsGeneric(typeof (IFoo<>))
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

        class Bar : IFoo<int>
        {
            #region IFoo<int> Members

            public int Get()
            {
                return 1;
            }

            #endregion
        }

        #endregion

        #region Nested type: ClosedType

        private class ClosedType : IClosedInterface
        {
            #region IClosedInterface Members

            public int Bar()
            {
                return 1;
            }

            #endregion
        }

        #endregion

        #region Nested type: IClosedInterface

        private interface IClosedInterface : IGenericInterface<int>
        {
        }

        #endregion

        #region Nested type: IFoo

        interface IFoo<out T>
        {
            T Get();
        }

        #endregion

        #region Nested type: IGenericInterface

        private interface IGenericInterface<out T>
        {
            T Bar();
        }

        #endregion
    }
}