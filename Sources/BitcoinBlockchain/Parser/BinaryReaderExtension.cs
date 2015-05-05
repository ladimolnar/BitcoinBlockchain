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
            const int bufferMaxSize = 1000000;

            int peekCharResult;
            if (binaryReader.BaseStream.CanSeek)
            {
                int skipedBytes = 0;
                while ((peekCharResult = binaryReader.PeekChar()) == 0 && skipedBytes < 1000)
                {
                    binaryReader.ReadBytes(1);
                    skipedBytes++;
                }

                if (peekCharResult != 0)
                {
                    return peekCharResult > 0;
                }

                // We already read 1000 zero bytes. Chances are that the remaining file contains only 0. 
                // We'll start reading larger blocks and seek to the correct position as needed.
                bool bufferIsZeroed;
                bool endOfFileReached;

                do
                {
                    long streamPosition = binaryReader.BaseStream.Position;

                    byte[] buffer = binaryReader.ReadBytes(bufferMaxSize);

                    endOfFileReached = buffer.Length < bufferMaxSize;

                    bufferIsZeroed = true;
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        if (buffer[i] != 0)
                        {
                            bufferIsZeroed = false;
                            binaryReader.BaseStream.Seek(streamPosition + i, SeekOrigin.Begin);
                            break;
                        }
                    }
                }
                while (bufferIsZeroed && endOfFileReached == false);

                return (endOfFileReached && bufferIsZeroed) == false;
            }
            else
            {
                while ((peekCharResult = binaryReader.PeekChar()) == 0)
                {
                    binaryReader.ReadBytes(1);
                }

                return peekCharResult > 0;
            }
        }
    }
}
