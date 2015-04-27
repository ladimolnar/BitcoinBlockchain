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
        /// A reference to the byte array parameter that was provided.
        /// Note that this is not a reference to a new byte array, it is a reference
        /// to the same byte array that was provided and that at return time has its 
        /// content reversed.
        /// </returns>
        public static byte[] ReverseByteArray(this byte[] byteArray)
        {
            Array.Reverse(byteArray);
            return byteArray;
        }
    }
}
