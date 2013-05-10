namespace Alphacloud.Common.Core.Reflection
{
    #region using

    using System;
    using System.Linq.Expressions;
    using System.Text;

    using Alphacloud.Common.Core.Data;

    using JetBrains.Annotations;

    #endregion

    /// <summary>
    ///     Gets property name using lambda expressions.
    /// </summary>
    [PublicAPI]
    public static class Property
    {
        public static string PropertyName <T>(this T obj, Expression<Func<T, object>> expression)
        {
            var body = expression.Body;
            return GetMemberName(new StringBuilder(128), body, false).ToString();
        }

        public static string Name <T>(
            Expression<Func<T, object>> expression)
        {
            var body = expression.Body;
            return GetMemberName(new StringBuilder(128), body, false).ToString();
        }


        public static string Name(
            Expression<Func<object>> expression, bool skipInstanceReference = true)
        {
            return GetMemberName(new StringBuilder(128), expression.Body, skipInstanceReference).ToString();
        }


        static StringBuilder GetMemberName(StringBuilder sb, Expression expression, bool skipInstanceReference)
        {
            var memberExpression = expression as MemberExpression;
            if (memberExpression != null)
            {
                if (memberExpression.Expression.NodeType ==
                    ExpressionType.MemberAccess)
                {
                    GetMemberName(sb, memberExpression.Expression, skipInstanceReference);
                    if (sb.Length > 0)
                        sb.Append(".");
                    return sb.Append(memberExpression.Member.Name);
                }
                if (skipInstanceReference && memberExpression.Expression.NodeType == ExpressionType.Constant)
                    return sb;
                return sb.Append(memberExpression.Member.Name);
            }

            var unaryExpression = expression as UnaryExpression;
            if (unaryExpression != null)
            {
                if (unaryExpression.NodeType != ExpressionType.Convert)
                    throw new Exception("Cannot interpret member from {0}".ApplyArgs(expression.ToString()));

                return GetMemberName(sb, unaryExpression.Operand, skipInstanceReference);
            }

            throw new Exception("Could not determine member from {0}".ApplyArgs(expression.ToString()));
        }
    }
}
