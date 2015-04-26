//-----------------------------------------------------------------------
// <copyright file="BlockProcessedEventArgs.cs">
// Copyright © Ladislau Molnar. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BitcoinBlockchain.Parser
{
    using System;
    using BitcoinBlockchain.Data;

    /// <summary>
    /// The argument of the event raised after the processing of a Bitcoin block completes.
    /// </summary>
    public class BlockProcessedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockProcessedEventArgs" /> class.
        /// </summary>
        /// <param name="block">
        /// Contains data describing the block that was processed.
        /// </param>
        public BlockProcessedEventArgs(Block block)
        {
            this.Block = block;
        }

        /// <summary>
        /// Gets the data describing the block that was processed.
        /// </summary>
        public Block Block { get; private set; }
    }
}
