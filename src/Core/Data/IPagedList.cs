/*
 ASP.NET MvcPager control
 Copyright:2009-2011 Webdiyer (http://en.webdiyer.com)
 Source code released under Ms-PL license
 */

namespace Alphacloud.Common.Core.Data
{
    /// <summary>
    ///   Paged list
    /// </summary>
    public interface IPagedList
    {
        /// <summary>
        ///   Current page idnex.
        /// </summary>
        int CurrentPageIndex { get; set; }

        /// <summary>
        ///   Page size.
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        ///   Total number of items.
        /// </summary>
        int TotalItemCount { get; set; }

        /// <summary>
        ///   Total page count.
        /// </summary>
        int TotalPageCount { get; }

        /// <summary>
        ///   Calculate start item index for current page.
        /// </summary>
        /// <returns>Start item index (0-based).</returns>
        int GetStartRecordIndex();

        /// <summary>
        ///   Calculate end item index for current page.
        /// </summary>
        /// <returns>End item index (0-based).</returns>
        int GetEndRecordIndex();
    }
}