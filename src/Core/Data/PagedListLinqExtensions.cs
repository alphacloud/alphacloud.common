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

    using JetBrains.Annotations;

    /// <summary>
    /// LINQ extensions for paged lists.
    /// </summary>
    [PublicAPI]
    public static class PagedListLinqExtensions
    {
        /// <summary>
        /// Get page from queriable.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="query">IQueryable</param>
        /// <param name="pageIndex">Page index (1-based).</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Paged list with given page loaded.</returns>
        public static PagedList<T> GetPage <T>
            (
            this IQueryable<T> query,
            int pageIndex,
            int pageSize
            )
        {
            if (pageIndex < 1)
                pageIndex = 1;
            var itemIndex = (pageIndex - 1) * pageSize;
            var pageOfItems = query.Skip(itemIndex).Take(pageSize);
            var totalItemCount = query.Count();
            return new PagedList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
        }


        /// <summary>
        ///   Get page of data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="pagingInfo">The pageing info.</param>
        /// <returns>Data page</returns>
        public static IEnumerable<T> GetPage <T>([NotNull] this IEnumerable<T> source, [NotNull] PagingInfo pagingInfo)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (pagingInfo == null)
                throw new ArgumentNullException("pagingInfo");

            return source.Skip(pagingInfo.StartIndex).Take(pagingInfo.Size);
        }
    }
}
