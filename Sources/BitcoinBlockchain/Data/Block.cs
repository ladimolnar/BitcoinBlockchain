//-----------------------------------------------------------------------
// <copyright file="Block.cs">
// Copyright © Ladislau Molnar. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BitcoinBlockchain.Data
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Contains information about a Bitcoin block.
    /// For more information see: https://en.bitcoin.it/wiki/Block
    /// </summary>
    public class Block
    {
        /// <summary>
        /// The list of transactions in this Bitcoin block.
        /// </summary>
        private readonly List<Transaction> transactions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Block"/> class.
        /// </summary>
        /// <param name="blockchainFileName">
        /// The name of the Bitcoin blockchain file that contains this block.
        /// </param>
        /// <param name="blockHeader">
        /// The block header.
        /// </param>
        public Block(string blockchainFileName, BlockHeader blockHeader)
        {
            this.BlockchainFileName = blockchainFileName;
            this.BlockHeader = blockHeader;

            this.transactions = new List<Transaction>();
            this.Transactions = new ReadOnlyCollection<Transaction>(this.transactions);
        }

        /// <summary>
        /// Gets the name of the blockchain file that contains the block being parsed.
        /// </summary>
        public string BlockchainFileName { get; private set; }

        /// <summary>
        /// Gets or sets a percentage indicating how much of the current blockchain file was processed.
        /// </summary>
        public int PercentageOfCurrentBlockchainFile { get; set; }

        /// <summary>
        /// Gets the block header information.
        /// </summary>
        public BlockHeader BlockHeader { get; private set; }

        /// <summary>
        /// Gets the read-only collection of transactions in this Bitcoin block.
        /// </summary>
        public ReadOnlyCollection<Transaction> Transactions { get; private set; }

        /// <summary>
        /// Gets the total count of the transaction inputs in this block.
        /// </summary>
        public int TransactionInputsCount
        {
            get { return this.Transactions.Sum(t => t.Inputs.Count); }
        }

        /// <summary>
        /// Gets the total count of the transaction outputs in this block.
        /// </summary>
        public int TransactionOutputsCount
        {
            get { return this.Transactions.Sum(t => t.Outputs.Count); }
        }

        /// <summary>
        /// Adds a new transaction to the list of transactions.
        /// </summary>
        /// <param name="transaction">
        /// The Bitcoin transaction to be added to the list of transactions.
        /// </param>
        public void AddTransaction(Transaction transaction)
        {
            this.transactions.Add(transaction);
        }
    }
}
