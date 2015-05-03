//-----------------------------------------------------------------------
// <copyright file="TransactionOutput.cs">
// Copyright © Ladislau Molnar. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BitcoinBlockchain.Data
{
    using System;

    /// <summary>
    /// Contains information about a Bitcoin transaction output.
    /// For more information see:
    /// <c>https://en.bitcoin.it/wiki/Transaction#general_format_.28inside_a_block.29_of_each_output_of_a_transaction_-_Txout</c>
    /// </summary>
    public class TransactionOutput
    {
        /// <summary>
        /// Gets or sets the value for this output in Satoshi.
        /// </summary>
        public UInt64 OutputValueSatoshi { get; set; }

        /// <summary>
        /// Gets or sets the output's script.
        /// </summary>
        public ByteArray OutputScript { get; set; }
    }
}
