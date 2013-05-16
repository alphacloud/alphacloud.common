namespace Alphacloud.Common.Core.Data
{
    using System;
    using System.Text;
    using JetBrains.Annotations;

    [PublicAPI]
    public static class BinaryFormattingExtensions
    {
        /// <summary>
        ///   Convert byte to binary strign representation.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public static string AsBinary(this byte val)
        {
            return ConvertToBinaryString(val, sizeof (byte)*8);
        }


        /// <summary>
        ///   Format integer as a binary string with spaces between every 8 bits
        /// </summary>
        /// <param name="input">integer to format</param>
        /// <param name="bits">Number of bits to process</param>
        /// <returns>string of the form "00000000 00000000 00000110 00011000"</returns>
        private static string ConvertToBinaryString(ulong input, int bits)
        {
            if (bits < 4)
                throw new ArgumentOutOfRangeException("bits", bits, @"Bit count should be in 4..64 range");

            var bitstring = new StringBuilder(bits + bits/8);
            for (var i = bits - 1; i >= 0; i--)
            {
                var result = ((input >> i) & 1) == 1 ? '1' : '0';
                bitstring.Append(result);
                if ((i > 0) && ((i)%8) == 0)
                    bitstring.Append(' ');
            }
            return bitstring.ToString();
        }
    }
}