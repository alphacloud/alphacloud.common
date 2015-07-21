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

namespace Alphacloud.Common.Core.Utils
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    ///   Maybe monad implementation.
    ///   Taken from http://www.codeproject.com/Articles/109026/Chained-null-checks-and-the-Maybe-monad.
    /// </summary>
    /// <remarks>See http://en.wikipedia.org/wiki/Monad_(functional_programming)#Maybe_monad for details.</remarks>
    [PublicAPI]
    public static class MaybeMonad
    {
        /// <summary>
        ///   Takes a function which defines the next value in the chain. If we pass null, we get null back
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="op">Value to test</param>
        /// <param name="evaluator"></param>
        /// <returns></returns>
        public static TResult With<TInput, TResult>(
            [CanBeNull] this TInput op,
            [NotNull] Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class
        {
            if (op == null)
            {
                return null;
            }
            if (evaluator == null)
            {
                throw new ArgumentNullException("evaluator");
            }
            return evaluator(op);
        }


        /// <summary>
        ///   Evaluates value of input <paramref name="op"/> if it is not null.
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="op"></param>
        /// <param name="evaluator">Evaluation callback.</param>
        /// <param name="failureValue">Default value.</param>
        /// <returns>
        ///   Evaluation result if <paramref name="op"/> is assigned; <paramref name="failureValue" /> otherwise.
        /// </returns>
        public static TResult Return<TInput, TResult>(
            [CanBeNull] this TInput op, [NotNull] Func<TInput, TResult> evaluator, TResult failureValue)
            where TInput : class
        {
            if (op == null)
            {
                return failureValue;
            }
            if (evaluator == null)
            {
                throw new ArgumentNullException("evaluator");
            }
            return evaluator(op);
        }

        /// <summary>
        /// Continue chain if expression evaluates to <c>true</c>.
        /// Returns <paramref name="op"/> if expression evaluates to <c>true</c>.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <param name="op">The input value.</param>
        /// <param name="evaluator">The evaluator.</param>
        /// <returns><paramref name="op"/> if expression evaluaes to <c>true</c>; <c>null</c> otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">evaluator</exception>
        public static TInput If<TInput>(this TInput op, [NotNull] Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (op == null)
            {
                return null;
            }
            if (evaluator == null)
            {
                throw new ArgumentNullException("evaluator");
            }
            return evaluator(op) ? op : null;
        }


        /// <summary>
        /// Break chain if expression evaluates to <c>true</c>.
        /// Returns <c>null</c> if expression evaluates to true.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <param name="op">The input value.</param>
        /// <param name="evaluator">The evaluator.</param>
        /// <returns><c>null</c> if expression evaluates to <c>true</c>; <paramref name="op"/> otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">evaluator</exception>
        public static TInput Unless<TInput>(this TInput op, [NotNull] Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (op == null)
            {
                return null;
            }
            if (evaluator == null)
            {
                throw new ArgumentNullException("evaluator");
            }
            return evaluator(op) ? null : op;
        }


        /// <summary>
        ///   Performs action if source is assigned.
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="o"></param>
        /// <param name="action"></param>
        /// <returns>
        ///   Source (<see paramref="o" />) or <c>null</c>
        /// </returns>
        public static TInput Do<TInput>(this TInput o, [NotNull] Action<TInput> action)
            where TInput : class
        {
            if (o == null)
            {
                return null;
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            action(o);
            return o;
        }
    }
}