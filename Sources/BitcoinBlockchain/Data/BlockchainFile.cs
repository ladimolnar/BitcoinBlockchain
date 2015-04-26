//-----------------------------------------------------------------------
// <copyright file="BlockchainFile.cs">
// Copyright © Ladislau Molnar. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BitcoinBlockchain.Data
{
    using System.IO;

    /// <summary>
    /// A class containing information about a Bitcoin blockchain file.
    /// </summary>
    public class BlockchainFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockchainFile" /> class.
        /// </summary>
        /// <param name="fileName">The name of the Bitcoin blockchain file.</param>
        /// <param name="binaryReader">A stream that can be used to read the Bitcoin blockchain file.</param>
        public BlockchainFile(string fileName, BinaryReader binaryReader)
        {
            this.FileName = fileName;
            this.BinaryReader = binaryReader;
        }

        /// <summary>
        /// Gets the name of the Bitcoin blockchain file.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets a stream that can be used to read the Bitcoin blockchain file.
        /// </summary>
        public BinaryReader BinaryReader { get; private set; }
    }
}
