using System;
using AccountView.Common.Base.Annotations;

namespace AccountView.Common.Base.Data
{
    /// <summary>
    ///     Maybe monad implementation.
    ///     Taken from http://www.codeproject.com/Articles/109026/Chained-null-checks-and-the-Maybe-monad.
    /// </summary>
    /// <remarks>See http://en.wikipedia.org/wiki/Monad_(functional_programming)#Maybe_monad for details.</remarks>
    public static class MaybeMonad
    {
        /// <summary>
        ///     Takes a function which defines the next value in the chain. If we pass null, we get null back
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="o"></param>
        /// <param name="evaluator"></param>
        /// <returns></returns>
        public static TResult With<TInput, TResult>(
            [CanBeNull] this TInput o,
            [NotNull] Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class
        {
            if (o == null)
            {
                return null;
            }
            if (evaluator == null)
            {
                throw new ArgumentNullException("evaluator");
            }
            return evaluator(o);
        }


        /// <summary>
        ///     Evaluates value of input <see cref="o" /> is not null.
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="o"></param>
        /// <param name="evaluator">Evaluation callback.</param>
        /// <param name="failureValue">Default value.</param>
        /// <returns>
        ///     Evaluation result if <see cref="o" /> is assigned; <see cref="failureValue" /> otherwise.
        /// </returns>
        public static TResult Return<TInput, TResult>(
            [CanBeNull] this TInput o, [NotNull] Func<TInput, TResult> evaluator, TResult failureValue)
            where TInput : class
        {
            if (o == null)
            {
                return failureValue;
            }
            if (evaluator == null)
            {
                throw new ArgumentNullException("evaluator");
            }
            return evaluator(o);
        }


        public static TInput If<TInput>(this TInput o, [NotNull] Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (o == null)
            {
                return null;
            }
            if (evaluator == null)
            {
                throw new ArgumentNullException("evaluator");
            }
            return evaluator(o) ? o : null;
        }


        public static TInput Unless<TInput>(this TInput o, [NotNull] Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (o == null)
            {
                return null;
            }
            if (evaluator == null)
            {
                throw new ArgumentNullException("evaluator");
            }
            return evaluator(o) ? null : o;
        }


        /// <summary>
        ///     Performs action if source is assigned.
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="o"></param>
        /// <param name="action"></param>
        /// <returns>
        ///     Source (<see cref="o" />) or <c>null</c>
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