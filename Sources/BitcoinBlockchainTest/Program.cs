//-----------------------------------------------------------------------
// <copyright file="Program.cs">
// Copyright © Ladislau Molnar. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BitcoinBlockchainTest
{
    using System;
    using BitcoinBlockchain.Parser;
    using ParserData = BitcoinBlockchain.Data;

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

            IBlockchainParser blockchainParser = new BlockchainParser(pathToBlockchain);

            // Start the parsing process by calling blockchainParser.ParseBlockchain() and
            // process each block that is returned by the parser.
            // The parser exposes the blocks it parses via an "IEnumerable<Block>".
            // ATENTION! Make sure that you are aware of the concept of orphan blocks.
            //           Depending on what your processing does, including the orphan blocks could lead
            //           to incorrect results. Detecting orphan blocks is not facilitated by the parser,
            //           you will have to do that on your own.
            foreach (ParserData.Block block in blockchainParser.ParseBlockchain())
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
            Console.WriteLine("This is a sample/test application used to exercise");
            Console.WriteLine("basic functions of the BitcoinBlockchain class library.");
        }
    }
}
