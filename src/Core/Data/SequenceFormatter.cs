namespace Alphacloud.Common.Core.Data
{
    #region using

    using System.Collections;
    using System.Text;

    using JetBrains.Annotations;

    #endregion

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
        ///     Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
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
