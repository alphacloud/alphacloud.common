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
    using System.Text;

    using JetBrains.Annotations;

    /// <summary>
    ///   Format number as binary.
    /// </summary>
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
            return ConvertToBinaryString(val, sizeof (byte) * 8);
        }


        /// <summary>
        ///   Format integer as a binary string with spaces between every 8 bits
        /// </summary>
        /// <param name="input">integer to format</param>
        /// <param name="bits">Number of bits to process</param>
        /// <returns>string of the form "00000000 00000000 00000110 00011000"</returns>
        static string ConvertToBinaryString(ulong input, int bits)
        {
            if (bits < 4)
                throw new ArgumentOutOfRangeException("bits", bits, @"Bit count should be in 4..64 range");

            var bitstring = new StringBuilder(bits + bits / 8);
            for (var i = bits - 1; i >= 0; i--)
            {
                var result = ((input >> i) & 1) == 1 ? '1' : '0';
                bitstring.Append(result);
                if ((i > 0) && ((i) % 8) == 0)
                    bitstring.Append(' ');
            }
            return bitstring.ToString();
        }
    }
}
