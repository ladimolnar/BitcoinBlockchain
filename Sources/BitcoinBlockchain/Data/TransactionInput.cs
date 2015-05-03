//-----------------------------------------------------------------------
// <copyright file="TransactionInput.cs">
// Copyright © Ladislau Molnar. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BitcoinBlockchain.Data
{
    using System;

    /// <summary>
    /// Contains information about a Bitcoin transaction input.
    /// For more information see:
    /// <c>https://en.bitcoin.it/wiki/Transaction#general_format_.28inside_a_block.29_of_each_input_of_a_transaction_-_Txin</c>
    /// </summary>
    public class TransactionInput
    {
        /// <summary>
        /// Value used for SourceTransactionOutputIndex to indicate that the input refers to no previous output.
        /// </summary>
        public const UInt32 OutputIndexNotUsed = 0xFFFFFFFF;

        /// <summary>
        /// Gets or sets the input's script.
        /// </summary>
        public ByteArray InputScript { get; set; }

        /// <summary>
        /// Gets or sets a double SHA256 hash that identifies the transaction that has the output that will be consumed by this input.
        /// This ByteArray instance contains the hash in reverse order from what is the 
        /// normal result of hashing. This is to be consistent with sites like
        /// blockchain.info and blockexporer that display hashes in 'big endian' format.
        /// </summary>
        public ByteArray SourceTransactionHash { get; set; }

        /// <summary>
        /// Gets or sets the index of the output that will be consumed by this input.
        /// The index is a zero based index in the list of outputs of the transaction that it belongs to.
        /// It can be OutputIndexNotUsed to indicate that this input refers to no previous output.
        /// </summary>
        public UInt32 SourceTransactionOutputIndex { get; set; }
    }
}
