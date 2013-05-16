/*
 ASP.NET MvcPager control
 Copyright:2009-2011 Webdiyer (http://en.webdiyer.com)
 Source code released under Ms-PL license
 */

namespace Alphacloud.Common.Core.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;

    /// <summary>
    ///   List with paging support.
    /// </summary>
    /// <typeparam name="T">Item type. </typeparam>
    public class PagedList<T> : List<T>, IPagedList
    {
        #region .ctor

        /// <summary>
        ///   Extract page form source and create list.
        /// </summary>
        /// <param name="items"> source </param>
        /// <param name="pageIndex"> Page index to extract </param>
        /// <param name="pageSize"> </param>
        public PagedList(IList<T> items, int pageIndex, int pageSize)
        {
            PageSize = pageSize;
            TotalItemCount = items.Count;
            CurrentPageIndex = pageIndex;
            var endRecordIndex = GetEndRecordIndex();
            for (int i = GetStartRecordIndex(); i <= endRecordIndex; i++)
                Add(items[i]);
        }


        /// <summary>
        ///   Create list for particular page.
        /// </summary>
        /// <param name="items"> </param>
        /// <param name="pageIndex"> </param>
        /// <param name="pageSize"> </param>
        /// <param name="totalItemCount"> </param>
        public PagedList(IEnumerable<T> items, int pageIndex, int pageSize, int totalItemCount)
        {
            AddRange(items);
            TotalItemCount = totalItemCount;
            CurrentPageIndex = pageIndex;
            PageSize = pageSize;
        }


        /// <summary>
        ///   Create list representing given page of data..
        /// </summary>
        /// <param name="items"> Page data. </param>
        /// <param name="page"> Page information. </param>
        /// <param name="totalItemCount"> Total number of items. </param>
        public PagedList(IEnumerable<T> items, PagingInfo page, int totalItemCount)
        {
            AddRange(items);
            TotalItemCount = totalItemCount;
            CurrentPageIndex = page.Number;
            PageSize = page.Size;
        }

        #endregion

        #region IPagedList Members

        /// <summary>
        ///   Total number of pages.
        /// </summary>
        public int TotalPageCount
        {
            get { return (int) Math.Ceiling(TotalItemCount/(double) PageSize); }
        }

        /// <summary>
        ///   Current page indeg.
        /// </summary>
        public int CurrentPageIndex { get; set; }

        /// <summary>
        ///   Page size.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        ///   Total number of items.
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        ///   Calculate start item index for current page.
        /// </summary>
        /// <returns>Start item index (0-based).</returns>
        public int GetStartRecordIndex()
        {
            return (CurrentPageIndex - 1)*PageSize;
        }


        /// <summary>
        ///   Calculate end item index for current page.
        /// </summary>
        /// <returns>Start item index (0-based).</returns>
        public int GetEndRecordIndex()
        {
            var idx = CurrentPageIndex*PageSize - 1;
            return idx >= TotalItemCount - 1 ? TotalItemCount - 1 : idx;
        }

        #endregion

        /// <summary>
        ///   Transform from other list.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static PagedList<T> CreateFrom<TSource>(PagedList<TSource> source) where TSource : T
        {
            var data = new List<T>(source.Cast<T>());
            return new PagedList<T>(data, source.CurrentPageIndex, source.PageSize, source.TotalItemCount);
        }


        /// <summary>
        ///   Convert from existing list.
        /// </summary>
        /// <typeparam name="TSource"> Source type. </typeparam>
        /// <param name="source"> Source list. </param>
        /// <param name="selector"> Type convertor. </param>
        /// <returns> New list. </returns>
        public static PagedList<T> ConvertFrom<TSource>(PagedList<TSource> source, Func<TSource, T> selector)
        {
            var data = source.Select(selector);
            return new PagedList<T>(data, source.CurrentPageIndex, source.PageSize, source.TotalItemCount);
        }

        /// <summary>
        ///   Create single page from source.
        /// </summary>
        /// <param name="source">Source items</param>
        /// <returns></returns>
        public static PagedList<T> AsSinglePage([NotNull] ICollection<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            var count = source.Count;
            return new PagedList<T>(source, 1, count, count);
        }

        /// <summary>
        ///   Create empty paged list.
        /// </summary>
        /// <returns>New instance</returns>
        public static PagedList<T> Empty()
        {
            return new PagedList<T>(new T[0], 1, 1);
        }
    }
}