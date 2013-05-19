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

    using JetBrains.Annotations;

    /// <summary>
    ///   Contains paging information.
    ///   Page number is always zero-based; Item index is always 1-based.
    /// </summary>
    [PublicAPI]
    [Serializable]
    public class PagingInfo : IEquatable<PagingInfo>
    {
        int _number;
        int _size;

        #region .ctor

        public PagingInfo() : this(10, 1)
        {
        }


        /// <summary>
        ///   Initialize new paging info with specified size and page index equal to 1.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        public PagingInfo(int pageSize) : this(pageSize, 1)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="PagingInfo" /> class.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page (1-based).</param>
        public PagingInfo(int pageSize, int pageIndex)
        {
            Size = pageSize;
            Number = pageIndex;
        }


        /// <summary>
        ///   Select top <paramref name="count" /> of items.
        /// </summary>
        /// <param name="count">Number of items to select.</param>
        /// <returns>PagingInfo structure</returns>
        public static PagingInfo Top(int count)
        {
            return new PagingInfo(count, 1);
        }

        #endregion

        /// <summary>
        ///   Gets or sets the size of the page.
        /// </summary>
        /// <value>
        ///   The size of the page.
        /// </value>
        public int Size
        {
            get { return _size; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value", value, @"Page Size must be greater than zero.");

                _size = value;
            }
        }

        /// <summary>
        ///   Gets or sets the index of the page.
        /// </summary>
        /// <value>
        ///   The index of the page (1-based).
        /// </value>
        public int Number
        {
            get { return _number; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", value, @"Page Index must be greater than zero.");

                _number = value;
            }
        }


        /// <summary>
        ///   Gets the start index in 0-based sequence.
        /// </summary>
        public int StartIndex
        {
            get { return (Number - 1) * Size; }
        }

        /// <summary>
        ///   Gets the end index in 0-based sequence.
        /// </summary>
        public int EndIndex
        {
            get { return Number * Size - 1; }
        }


        public bool Equals(PagingInfo other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return other._number == _number && other._size == _size;
        }

        /// <summary>
        ///   Gets the page number for item.
        /// </summary>
        /// <param name="itemIndex">
        ///   <strong>Zero-based</strong>index of the item.
        /// </param>
        /// <returns>1-based Page number.</returns>
        public int GetPageNumberByIndex(int itemIndex)
        {
            if (itemIndex < 0)
                throw new ArgumentOutOfRangeException("itemIndex", itemIndex, @"Item index must bepositive number.");
            return (itemIndex / Size) + 1;
        }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof (PagingInfo))
                return false;
            return Equals((PagingInfo) obj);
        }


        public override int GetHashCode()
        {
            unchecked
            {
                return (_number * 397) ^ _size;
            }
        }


        public static bool operator ==(PagingInfo left, PagingInfo right)
        {
            return Equals(left, right);
        }


        public static bool operator !=(PagingInfo left, PagingInfo right)
        {
            return !Equals(left, right);
        }

        #region Overrides of Object

        public override string ToString()
        {
            return "Page {1} of size {0}".ApplyArgs(Size, Number);
        }

        #endregion
    }
}
