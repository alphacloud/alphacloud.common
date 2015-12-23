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
            var body = expression.Body;
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
            var body = expression.Body;
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


        static StringBuilder GetMemberName(StringBuilder sb, Expression expression, bool skipInstanceReference)
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
                        "Could not determine member from expression '{0}'.".ApplyArgs(expression));

                if (unaryExpression.NodeType != ExpressionType.Convert)
                    throw new InvalidOperationException(
                        "Cannot interpret member from expression '{0}'.".ApplyArgs(expression));

                expression = unaryExpression.Operand;
            }
        }
    }
}