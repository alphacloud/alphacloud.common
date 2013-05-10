namespace Alphacloud.Common.Core.Data
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using JetBrains.Annotations;

    #endregion

    [PublicAPI]
    public static class PagedListLinqExtensions
    {
        public static PagedList<T> ToPagedList <T>
            (
            this IQueryable<T> allItems,
            int pageIndex,
            int pageSize
            )
        {
            if (pageIndex < 1)
                pageIndex = 1;
            var itemIndex = (pageIndex - 1) * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);
            var totalItemCount = allItems.Count();
            return new PagedList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
        }


        /// <summary>
        ///     Get page of data.
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
