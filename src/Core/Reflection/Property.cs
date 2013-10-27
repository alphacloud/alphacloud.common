namespace Alphacloud.Common.Core.Reflection
{
    using System;
    using System.Linq.Expressions;
    using System.Text;
    using Data;
    using JetBrains.Annotations;

    /// <summary>
    ///   Retrieves text property name from lambda expression.
    /// </summary>
    /// <remarks>
    ///   Nested properties are supported.
    ///   Indexes are NOT supported.
    /// </remarks>
    [PublicAPI]
    public static class Property
    {
        /// <summary>
        ///   Get property name from expression.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="obj">Object instance.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>Property name.</returns>
        public static string PropertyName<T>(this T obj, Expression<Func<T, object>> expression)
        {
            Expression body = expression.Body;
            return GetMemberName(new StringBuilder(128), body, false).ToString();
        }

        /// <summary>
        ///   Get property name from expression.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>Textual property name.</returns>
        public static string Name<T>(
            Expression<Func<T, object>> expression)
        {
            Expression body = expression.Body;
            return GetMemberName(new StringBuilder(128), body, false).ToString();
        }


        /// <summary>
        ///   Get property name from expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="skipInstanceReference">
        ///   if set to <c>true</c> instance variable won't be included in property name.
        /// </param>
        /// <returns></returns>
        /// <example>
        ///   <c>Property.Name(() => obj.Name, false)</c> will return <c>obj.Name</c>
        ///   while
        ///   <c>Property.Name(() => obj.Name, true)</c> will return <c>Name</c>
        /// </example>
        public static string Name(
            Expression<Func<object>> expression, bool skipInstanceReference = true)
        {
            return GetMemberName(new StringBuilder(128), expression.Body, skipInstanceReference).ToString();
        }


        private static StringBuilder GetMemberName(StringBuilder sb, Expression expression, bool skipInstanceReference)
        {
            while (true)
            {
                var memberExpression = expression as MemberExpression;
                if (memberExpression != null)
                {
                    if (memberExpression.Expression.NodeType == ExpressionType.MemberAccess)
                    {
                        GetMemberName(sb, memberExpression.Expression, skipInstanceReference);
                        if (sb.Length > 0)
                            sb.Append('.');
                        return sb.Append(memberExpression.Member.Name);
                    }
                    if (skipInstanceReference && memberExpression.Expression.NodeType == ExpressionType.Constant)
                        return sb;
                    return sb.Append(memberExpression.Member.Name);
                }

                var unaryExpression = expression as UnaryExpression;
                if (unaryExpression == null)
                    throw new InvalidOperationException(
                        "Could not determine member from {0}".ApplyArgs(expression));

                if (unaryExpression.NodeType != ExpressionType.Convert)
                    throw new InvalidOperationException(
                        "Cannot interpret member from {0}".ApplyArgs(expression));

                expression = unaryExpression.Operand;
            }
        }
    }
}