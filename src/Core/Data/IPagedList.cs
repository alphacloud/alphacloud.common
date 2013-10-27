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
