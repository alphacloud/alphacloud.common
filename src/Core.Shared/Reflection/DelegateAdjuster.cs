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

namespace Alphacloud.Common.Core.Reflection
{
    using System;
    using System.Linq.Expressions;
    using JetBrains.Annotations;

    /// <summary>
    ///   Adjust delegate arguments.
    /// </summary>
    /// <remarks>
    ///   Based on DelegateAdjuster from
    ///   https://github.com/gregoryyoung/m-r/blob/master/SimpleCQRS/InfrastructureCrap.DontBotherReadingItsNotImportant.cs
    /// </remarks>
    [PublicAPI]
    public static class DelegateAdjuster
    {
        /// <summary>
        ///   Creates delegate which accpepts argument of base class, dynamically upcasts it calls given action.
        /// </summary>
        /// <typeparam name="TBase">Base type for the argument.</typeparam>
        /// <typeparam name="TDerived">Required type for the argument.</typeparam>
        /// <param name="source"></param>
        /// <returns>Delegate of type <c>Action&lt;TBase&gt;</c></returns>
        public static Action<TBase> CastArgument<TBase, TDerived>(Expression<Action<TDerived>> source)
            where TDerived : TBase
        {
            if (typeof (TDerived) == typeof (TBase))
                return (Action<TBase>) (Delegate) source.Compile();
            var sourceParameter = Expression.Parameter(typeof (TBase), "source");
            var result = Expression.Lambda<Action<TBase>>(
                Expression.Invoke(
                    source,
                    Expression.Convert(sourceParameter, typeof (TDerived))),
                sourceParameter);
            return result.Compile();
        }


        /// <summary>
        /// Creates delegate which accpepts first argument of base class, dynamically upcasts it calls given action.
        /// </summary>
        /// <typeparam name="TBase">Base type for the first argument.</typeparam>
        /// <typeparam name="TDerived">Actual type for the first argument.</typeparam>
        /// <typeparam name="TArg2">Second Argument type.</typeparam>
        /// <param name="source">The source expression.</param>
        /// <returns>Delegate with first argument of <typeparamref name="TBase"/></returns>
        public static Action<TBase, TArg2> CastFirstArgument<TBase, TDerived, TArg2>(
            Expression<Action<TDerived, TArg2>> source)
            where TDerived : TBase
        {
            if (typeof (TDerived) == typeof (TBase))
                return (Action<TBase, TArg2>) (Delegate) source.Compile();
            var sourceParameter = Expression.Parameter(typeof (TBase), "source");
            var arg1 = Expression.Parameter(typeof (TArg2), "arg1");
            var result = Expression.Lambda<Action<TBase, TArg2>>(
                Expression.Invoke(
                    source,
                    Expression.Convert(sourceParameter, typeof (TDerived)),
                    arg1),
                sourceParameter, arg1);
            return result.Compile();
        }


        /// <summary>
        /// Creates delegate which accpepts first argument of base class, dynamically upcasts it calls given function.
        /// </summary>
        /// <typeparam name="TBase">Base type for the first argument.</typeparam>
        /// <typeparam name="TDerived">Requred type for the first argument.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source expression.</param>
        /// <returns>Function with first argument of type <typeparamref name="TBase"/></returns>
        public static Func<TBase, TResult> CastArgument<TBase, TDerived, TResult>(
            Expression<Func<TDerived, TResult>> source)
            where TDerived : TBase
        {
            if (typeof (TDerived) == typeof (TBase))
                return (Func<TBase, TResult>) (Delegate) source.Compile();
            var sourceParameter = Expression.Parameter(typeof (TBase), "source");
            var result = Expression.Lambda<Func<TBase, TResult>>(
                Expression.Invoke(
                    source,
                    Expression.Convert(sourceParameter, typeof (TDerived))),
                sourceParameter);
            return result.Compile();
        }
    }
}