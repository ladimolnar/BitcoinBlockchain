//-----------------------------------------------------------------------
// <copyright file="Program.cs">
// Copyright © Ladislau Molnar. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BitcoinBlockchainSample
{
    using System;
    using BitcoinBlockchain.Data;
    using BitcoinBlockchain.Parser;

    public static class Program
    {
        private static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("Invalid command line. Run \"BitcoinBlockchainSample /?\" for usage.");
            }
            else
            {
                if (args[0] == "/?")
                {
                    TypeHelp();
                }
                else
                {
                    ParseBlockchainFiles(args[0]);
                }
            }

            return 0;
        }

        private static void ParseBlockchainFiles(string pathToBlockchain)
        {
            BlockchainStatistics overallStatistics = new BlockchainStatistics();
            BlockchainStatistics blockFileStatistics = new BlockchainStatistics();

            string currentBlockchainFile = null;

            // Instantiate a BlockchainParser. We will pass the path to the blockchain files
            // to its constructor. 
            // TIP: Class IBlockchainParser provides several constructors that are useful 
            //      in different scenarios.
            IBlockchainParser blockchainParser = new BlockchainParser(pathToBlockchain);

            // Start the parsing process by calling blockchainParser.ParseBlockchain() and
            // process each block that is returned by the parser.
            // The parser exposes the blocks it parses via an "IEnumerable<Block>".
            // TIPS: 
            // 1. An instance of type BitcoinBlockchain.Data.Block holds information
            //    about all its transactions, inputs and outputs and it can use a lot of memory.
            //    After you are done processing a block do not keep it around in memory. 
            //    For example do not simply collect all instances of type BitcoinBlockchain.Data.Block 
            //    in a list. That would consume huge amounts of memory.
            //
            // 2. To improve the performance of your application you may want to dispatch the processing 
            //    of a block on a background thread. 
            //    If you do that however you need to account for the fact that multiple blocks will 
            //    be processed concurrently. You have to be prepared to deal with various multi-threading 
            //    aspects. For example a transaction input may end up being processed before the output 
            //    it links to. You may want to consider a hybrid approach where some of the processing 
            //    for a block is done on the main thread and some of the processing is dispatched on a 
            //    background thread.
            //
            // 3. If during processing you need to store so much information that you expect to
            //    exceed 2 GB of memory, build your tool for the x64 configuration.
            //
            // 4. Make sure that you are aware of the concept of stale blocks.
            //    Depending on what your processing does, not accounting for stale blocks could 
            //    lead to incorrect results. The parser has no way to know that a block is stale 
            //    when it encounters it. It will enumerate it to you and you will have the chance 
            //    to detect the stale blocks once the parsing of all blocks is complete. 
            //    See:
            //          https://bitcoin.org/en/developer-guide#orphan-blocks
            //          https://bitcoin.org/en/glossary/stale-block
            //          https://bitcoin.org/en/glossary/orphan-block
            //          http://bitcoin.stackexchange.com/questions/5859/what-are-orphaned-and-stale-blocks
            foreach (Block block in blockchainParser.ParseBlockchain())
            {
                if (currentBlockchainFile != block.BlockchainFileName)
                {
                    if (currentBlockchainFile != null)
                    {
                        ReportBlockChainStatistics(blockFileStatistics);
                        blockFileStatistics.Reset();
                    }

                    currentBlockchainFile = block.BlockchainFileName;

                    Console.WriteLine("Parsing file: {0}", block.BlockchainFileName);
                }

                blockFileStatistics.AddStatistics(1, block.Transactions.Count, block.TransactionInputsCount, block.TransactionOutputsCount);
                overallStatistics.AddStatistics(1, block.Transactions.Count, block.TransactionInputsCount, block.TransactionOutputsCount);
            }

            ReportBlockChainStatistics(blockFileStatistics);

            Console.WriteLine("=================================================");
            Console.WriteLine("Overall statistics:");
            ReportBlockChainStatistics(overallStatistics);
        }

        private static void ReportBlockChainStatistics(BlockchainStatistics blockFileStatistics)
        {
            Console.WriteLine("                 Blocks count: {0,14:n0}", blockFileStatistics.BlocksCount);
            Console.WriteLine("           Transactions count: {0,14:n0}", blockFileStatistics.TransactionsCount);
            Console.WriteLine("     Transaction inputs count: {0,14:n0}", blockFileStatistics.TransactionsInputCount);
            Console.WriteLine("    Transaction outputs count: {0,14:n0}", blockFileStatistics.TransactionsOutputCount);
            Console.WriteLine();
        }

        private static void TypeHelp()
        {
            Console.WriteLine("BitcoinBlockchainSample");
            Console.WriteLine("    This is a sample/test application used to exercise");
            Console.WriteLine("    basic functions of the BitcoinBlockchain class library.");
            Console.WriteLine();
            Console.WriteLine("USAGE:");
            Console.WriteLine("    BitcoinBlockchainSample.exe /? | Path_To_Blockchain_Files");
        }
    }
}
