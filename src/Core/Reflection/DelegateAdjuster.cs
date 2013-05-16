namespace Alphacloud.Common.Core.Reflection
{
    #region using

    using System;
    using System.Linq.Expressions;
    using JetBrains.Annotations;

    #endregion

    [PublicAPI]
    public static class DelegateAdjuster
    {
        public static Action<TBase> CastArgument<TBase, TDerived>(Expression<Action<TDerived>> source)
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


        public static Action<TBase, TArg1> CastFirstArgument<TBase, TDerived, TArg1>(
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


        public static Func<TBase, TResult> CastArgument<TBase, TDerived, TResult>(
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