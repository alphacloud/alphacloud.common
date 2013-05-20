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
        /// 
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <typeparam name="TDerived"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Action<TBase> CastArgument <TBase, TDerived>(Expression<Action<TDerived>> source)
            where TDerived : TBase
        {
            if (typeof (TDerived) == typeof (TBase))
                return (Action<TBase>) ((Delegate) source.Compile());
            var sourceParameter = Expression.Parameter(typeof (TBase), "source");
            var result = Expression.Lambda<Action<TBase>>(
                Expression.Invoke(
                    source,
                    Expression.Convert(sourceParameter, typeof (TDerived))),
                sourceParameter);
            return result.Compile();
        }


        public static Action<TBase, TArg1> CastFirstArgument <TBase, TDerived, TArg1>(
            Expression<Action<TDerived, TArg1>> source)
            where TDerived : TBase
        {
            if (typeof (TDerived) == typeof (TBase))
                return (Action<TBase, TArg1>) ((Delegate) source.Compile());
            var sourceParameter = Expression.Parameter(typeof (TBase), "source");
            var arg1 = Expression.Parameter(typeof (TArg1), "arg1");
            var result = Expression.Lambda<Action<TBase, TArg1>>(
                Expression.Invoke(
                    source,
                    Expression.Convert(sourceParameter, typeof (TDerived)),
                    arg1),
                sourceParameter, arg1);
            return result.Compile();
        }


        public static Func<TBase, TResult> CastArgument <TBase, TDerived, TResult>(
            Expression<Func<TDerived, TResult>> source)
            where TDerived : TBase
        {
            if (typeof (TDerived) == typeof (TBase))
                return (Func<TBase, TResult>) ((Delegate) source.Compile());
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
