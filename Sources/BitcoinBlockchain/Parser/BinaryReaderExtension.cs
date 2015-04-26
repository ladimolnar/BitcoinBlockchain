//-----------------------------------------------------------------------
// <copyright file="BinaryReaderExtension.cs">
// Copyright © Ladislau Molnar. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BitcoinBlockchain.Parser
{
    using System.IO;

    /// <summary>
    /// Provides extension methods for the <see cref="BinaryReader "/> class.
    /// </summary>
    internal static class BinaryReaderExtension
    {
        /// <summary>
        /// Skips all bytes with a value of 0. Advances the current position of the stream 
        /// to a position just before the next non zero byte or until the end of the file is reached.
        /// </summary>
        /// <param name="binaryReader">
        /// Provides access to a Bitcoin blockchain file.
        /// </param>
        /// <returns>
        /// true  - All the bytes with zero value were skipped and there is more data in the file.
        /// false - All the bytes with zero value were skipped and the end of file was reached.
        /// </returns>
        internal static bool SkipZeroBytes(this BinaryReader binaryReader)
        {
            int peekCharResult;
            do
            {
                peekCharResult = binaryReader.PeekChar();
                if (peekCharResult == 0)
                {
                    binaryReader.ReadBytes(1);
                }
            }
            while (peekCharResult == 0);

            return peekCharResult > 0;
        }
    }
}
