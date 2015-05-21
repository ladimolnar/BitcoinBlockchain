//-----------------------------------------------------------------------
// <copyright file="IBlockchainParser.cs">
// Copyright © Ladislau Molnar. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BitcoinBlockchain.Parser
{
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
    }
}
