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

namespace Alphacloud.Common.Core.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using JetBrains.Annotations;

    /// <summary>
    ///   Dynamic sort helper.
    ///   Sorts by string expression, nested properties are supported.
    /// </summary>
    /// <example>
    ///   list.OrderBy("SomeProperty");
    ///   list.OrderBy("SomeProperty DESC");
    ///   list.OrderBy("SomeProperty DESC, SomeOtherProperty");
    ///   list.OrderBy("SomeSubObject.SomeProperty ASC, SomeOtherProperty DESC");
    /// </example>
    public static class OrderByHelper
    {
        /// <summary>
        ///   Sort by string expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="orderBy">Sort expression, nested properties supported.</param>
        /// <returns>Sorted sequence.</returns>
        public static IEnumerable<T> OrderBy<T>([NotNull] this IEnumerable<T> enumerable, [NotNull] string orderBy)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable");
            }
            if (orderBy == null)
            {
                throw new ArgumentNullException("orderBy");
            }
            return enumerable.AsQueryable().OrderBy(orderBy).AsEnumerable();
        }


        /// <summary>
        ///   Sort by string expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="orderBy">Sort expression, nested properties supported.</param>
        /// <returns>Sorted sequence.</returns>
        public static IQueryable<T> OrderBy<T>([NotNull] this IQueryable<T> collection, [NotNull] string orderBy)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            if (orderBy == null)
            {
                throw new ArgumentNullException("orderBy");
            }
            foreach (var orderByInfo in ParseOrderBy(orderBy))
                collection = ApplyOrderBy(collection, orderByInfo);

            return collection;
        }


        private static IQueryable<T> ApplyOrderBy<T>(IQueryable<T> collection, OrderByInfo orderByInfo)
        {
            var props = orderByInfo.PropertyName.Split('.');
            Type type = typeof (T);

            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof (Func<,>).MakeGenericType(typeof (T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            string methodName;

            if (!orderByInfo.Initial && collection is IOrderedQueryable<T>)
            {
                methodName = orderByInfo.Direction == SortDirection.Ascending ? "ThenBy" : "ThenByDescending";
            }
            else
            {
                methodName = orderByInfo.Direction == SortDirection.Ascending ? "OrderBy" : "OrderByDescending";
            }

            //TODO: apply caching to the generic methodsinfos?
            return (IOrderedQueryable<T>) typeof (Queryable).GetMethods().Single(
                method => method.Name == methodName
                    && method.IsGenericMethodDefinition
                    && method.GetGenericArguments().Length == 2
                    && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof (T), type)
                .Invoke(null, new object[] {collection, lambda});
        }


        private static IEnumerable<OrderByInfo> ParseOrderBy(string orderBy)
        {
            if (String.IsNullOrEmpty(orderBy))
            {
                yield break;
            }

            var items = orderBy.Split(',');
            bool initial = true;
            foreach (string item in items)
            {
                var pair = item.Trim().Split(' ');

                if (pair.Length > 2)
                {
                    throw new ArgumentException(
                        String.Format(
                            "Invalid OrderBy string '{0}'. Order By Format: Property, Property2 ASC, Property2 DESC",
                            item));
                }

                string prop = pair[0].Trim();

                if (String.IsNullOrEmpty(prop))
                {
                    throw new ArgumentException(
                        "Invalid Property. Order By Format: Property, Property2 ASC, Property2 DESC");
                }

                var dir = SortDirection.Ascending;

                if (pair.Length == 2)
                {
                    dir = ("desc".Equals(pair[1].Trim(), StringComparison.OrdinalIgnoreCase)
                        ? SortDirection.Descending
                        : SortDirection.Ascending);
                }

                yield return new OrderByInfo {PropertyName = prop, Direction = dir, Initial = initial};

                initial = false;
            }
        }


        private class OrderByInfo
        {
            public string PropertyName { get; set; }
            public SortDirection Direction { get; set; }
            public bool Initial { get; set; }
        }

        private enum SortDirection
        {
            Ascending = 0,
            Descending = 1
        }
    }
}