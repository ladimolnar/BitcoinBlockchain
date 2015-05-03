//-----------------------------------------------------------------------
// <copyright file="Transaction.cs">
// Copyright © Ladislau Molnar. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BitcoinBlockchain.Data
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Contains information about a Bitcoin transaction.
    /// For more information see: https://en.bitcoin.it/wiki/Transaction
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// The list of transaction inputs.
        /// </summary>
        private readonly List<TransactionInput> transactionInputs;

        /// <summary>
        /// The list of transaction outputs.
        /// </summary>
        private readonly List<TransactionOutput> transactionOutputs;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction" /> class.
        /// </summary>
        public Transaction()
        {
            this.transactionInputs = new List<TransactionInput>();
            this.transactionOutputs = new List<TransactionOutput>();

            this.Inputs = new ReadOnlyCollection<TransactionInput>(this.transactionInputs);
            this.Outputs = new ReadOnlyCollection<TransactionOutput>(this.transactionOutputs);
        }

        /// <summary>
        /// Gets or sets the 256 bit hash of this transaction.
        /// The hash is calculated for the block of memory that contains the entire transaction, 
        /// from transaction version number to the transaction lock time.
        /// This ByteArray instance contains the hash in reverse order from what is the 
        /// normal result of hashing. This is to be consistent with sites like
        /// blockchain.info and blockexporer that display hashes in 'big endian' format.
        /// </summary>
        public ByteArray TransactionHash { get; set; }

        /// <summary>
        /// Gets or sets the transaction version.
        /// </summary>
        public UInt32 TransactionVersion { get; set; }

        /// <summary>
        /// Gets or sets the transaction lock time.
        /// If non-zero and sequence numbers are less than 0xFFFFFFFF then it represents 
        /// the block height or the timestamp when transaction is final.
        /// </summary>
        public UInt32 TransactionLockTime { get; set; }

        /// <summary>
        /// Gets the read-only collection of transaction inputs in this transaction.
        /// </summary>
        public ReadOnlyCollection<TransactionInput> Inputs { get; private set; }

        /// <summary>
        /// Gets the read-only collection of transaction outputs in this transaction.
        /// </summary>
        public ReadOnlyCollection<TransactionOutput> Outputs { get; private set; }

        /// <summary>
        /// Adds a new input to the list of transaction inputs.
        /// </summary>
        /// <param name="transactionInput">
        /// The transaction input to be added to the list of transaction inputs.
        /// </param>
        public void AddInput(TransactionInput transactionInput)
        {
            this.transactionInputs.Add(transactionInput);
        }

        /// <summary>
        /// Adds a new output to the list of transaction outputs.
        /// </summary>
        /// <param name="transactionOutput">
        /// The transaction output to be added to the list of transaction outputs.
        /// </param>
        public void AddOutput(TransactionOutput transactionOutput)
        {
            this.transactionOutputs.Add(transactionOutput);
        }
    }
}
