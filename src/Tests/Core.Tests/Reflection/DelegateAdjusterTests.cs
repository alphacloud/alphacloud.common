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
    using System;
    using Alphacloud.Common.Core.Reflection;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    internal class DelegateAdjusterTests
    {
        [Test]
        public void Action_BaseTypePassed_ShouldThrow_InvalidCastException()
        {
            Action<B> b = bp => bp.ToString();
            var upcast = DelegateAdjuster.CastArgument<A, B>(x => b(x));
            upcast(new B());

            Action baseTypePassed = () => upcast(new A());
            baseTypePassed.ShouldThrow<InvalidCastException>();
        }


        [Test]
        public void ActionWithTwoArguments_BaseTypePassed_ShouldThrow_InvalidCastException()
        {
            Action<B, int> b = (bp, x) => bp.ToString();
            var upcast = DelegateAdjuster.CastFirstArgument<A, B, int>((x, y) => b(x, y));
            upcast(new B(), 1);

            Action baseTypePassed = () => upcast(new A(), 1);
            baseTypePassed.ShouldThrow<InvalidCastException>();
        }


        [Test]
        public void Function_BaseTypePassed_ShouldThrow_InvalidCastException()
        {
            Func<B, string> fu = (B a) => a.ToString();

            var f = DelegateAdjuster.CastArgument<A, B, string>(x => fu(x));

            var result = f(new B());
            result.Should().Be(typeof (B).ToString());

            Action passA = () => result = f(new A());
            passA.ShouldThrow<InvalidCastException>();
        }

        #region Nested type: A

        class A
        {
        }

        #endregion

        #region Nested type: B

        class B : A
        {
        }

        #endregion
    }
}