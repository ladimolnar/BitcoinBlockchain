//-----------------------------------------------------------------------
// <copyright file="ByteArrayExtension.cs">
// Copyright © Ladislau Molnar. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BitcoinBlockchain.Parser
{
    using System;

    /// <summary>
    /// Contains methods extending the byte[] type.
    /// </summary>
    public static class ByteArrayExtension
    {
        /// <summary>
        /// Reverses the given byte array and returns a reference to it
        /// Note that the content of the original byte array is changed.
        /// </summary>
        /// <param name="byteArray">
        /// The byte array that will be reversed.
        /// </param>
        /// <returns>
        /// A reference to the same byte array that was provided in <param name="byteArray" /> that at the time of return is reversed.
        /// </returns>
        public static byte[] ReverseByteArray(this byte[] byteArray)
        {
            Array.Reverse(byteArray);
            return byteArray;
        }
    }
}
