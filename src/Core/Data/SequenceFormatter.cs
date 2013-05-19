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
    using System.Collections;
    using System.Text;

    using JetBrains.Annotations;

    [PublicAPI]
    public class SequenceFormatter
    {
        readonly IEnumerable _source;


        public SequenceFormatter(IEnumerable source)
        {
            _source = source;
            Separator = ", ";
        }

        public string Header { get; set; }

        public string Separator { get; set; }

        #region Overrides of Object

        /// <summary>
        ///   Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///   A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(Header))
                sb.Append(Header);
            if (_source == null)
                sb.Append("<null>");
            else
            {
                sb.Append('[');
                var hasItems = false;
                foreach (var obj in _source)
                {
                    var s = obj != null ? obj.ToString() : "<null>";
                    sb.Append(s);
                    sb.Append(Separator);
                    hasItems = true;
                }
                if (hasItems)
                {
                    // remove last separator
                    sb.Length -= Separator.Length;
                }
                sb.Append("]");
            }
            return sb.ToString();
        }

        #endregion
    }
}
