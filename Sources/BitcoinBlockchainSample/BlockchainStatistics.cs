//-----------------------------------------------------------------------
// <copyright file="BlockchainStatistics.cs">
// Copyright © Ladislau Molnar. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BitcoinBlockchainSample
{
    public class BlockchainStatistics
    {
        public long BlocksCount { get; private set; }

        public long TransactionsCount { get; private set; }

        public long TransactionsInputCount { get; private set; }

        public long TransactionsOutputCount { get; private set; }

        public void AddStatistics(long blocksCount, long transactionsCount, long transactionsInputCount, long transactionsOutputCount)
        {
            this.BlocksCount += blocksCount;
            this.TransactionsCount += transactionsCount;
            this.TransactionsInputCount += transactionsInputCount;
            this.TransactionsOutputCount += transactionsOutputCount;
        }

        public void Reset()
        {
            this.BlocksCount = 0;
            this.TransactionsCount = 0;
            this.TransactionsInputCount = 0;
            this.TransactionsOutputCount = 0;
        }
    }
}
