//-----------------------------------------------------------------------
// <copyright file="IBlockchainParser.cs">
// Copyright © Ladislau Molnar. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BitcoinBlockchain.Parser
{
    using System;
    using System.Collections.Generic;
    using BitcoinBlockchain.Data;

    /// <summary>
    /// The interface that defines the methods necessary to parse a Bitcoin blockchain.
    /// </summary>
    public interface IBlockchainParser
    {
        /// <summary>
        /// Parses the Bitcoin blockchain and returns a <see cref="IEnumerable&lt;Block&gt;"/>.
        /// Each element contains information about one Bitcoin block.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerable&lt;Block&gt;"/>.
        /// Each element contains information about one Bitcoin block.
        /// </returns>
        IEnumerable<Block> ParseBlockchain();

        /// <summary>
        /// Sets the value that will be used to check against the BlockId of each block.
        /// If this method is not called then the default value of 0xD9B4BEF9 will be used.
        /// </summary>
        /// <param name="blockId">The value that will be used to check against the BlockId of each block.</param>
        void SetBlockId(UInt32 blockId);
    }
}
