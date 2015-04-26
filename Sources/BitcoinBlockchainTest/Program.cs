//-----------------------------------------------------------------------
// <copyright file="Program.cs">
// Copyright © Ladislau Molnar. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BitcoinBlockchainTest
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
                Console.Error.WriteLine("Invalid command line. Run \"BitcoinBlockchainTest /?\" for usage.");
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
            // 1. Make sure that you are aware of the concept of orphan blocks.
            //    Depending on what your processing does, including the orphan blocks could lead
            //    to incorrect results. Detecting orphan blocks is not facilitated by the parser,
            //    you will have to do that on your own.
            // 2. An instance of type BitcoinBlockchain.Data.Block holds information
            //    about all its transactions, inputs and outputs and it can use a lot of memory.
            //    Tray to make sure that after you process a block, you do not keep it around in
            //    memory. For example do not simply collect instances of type 
            //    BitcoinBlockchain.Data.Block in a list. That would consume huge amounts of memory.
            // 3. If during processing you need to store so much information that you expect to
            //    exceed 2 GB of memory, make sure you build your tool for the x64 configuration.
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
            Console.WriteLine("BitcoinBlockchainTest");
            Console.WriteLine("    This is a sample/test application used to exercise");
            Console.WriteLine("    basic functions of the BitcoinBlockchain class library.");
            Console.WriteLine();
            Console.WriteLine("USAGE:");
            Console.WriteLine("    BitcoinBlockchainTest.exe /? | Path_To_Blockchain_Files");
        }
    }
}
