//-----------------------------------------------------------------------
// <copyright file="BlockchainParser.cs">
// Copyright © Ladislau Molnar. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BitcoinBlockchain.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using BitcoinBlockchain.Data;

    /// <summary>
    /// This class implements the IBlockchainParser interface. 
    /// It parses and effectively processes a set of real Bitcoin blockchain streams.
    /// </summary>
    public partial class BlockchainParser : IBlockchainParser
    {
        /// <summary>
        /// The file selector used to filter the blockchain files in the Bitcoin blockchain folder.
        /// </summary>
        private const string BlockFileSelector = @"blk?????.dat";

        /// <summary>
        /// The expected size for the block header section of a block.
        /// </summary>
        private const int ExpectedBlockHeaderBufferSize = 80;

        /// <summary>
        /// The "magic" ID of each Bitcoin block.
        /// </summary>
        private const UInt32 DefaultBlockMagicId = 0xD9B4BEF9;

        /// <summary>
        /// An enumerable providing access to a set of BlockchainFile instances, each representing a Bitcoin blockchain file.
        /// </summary>
        private readonly IEnumerable<BlockchainFile> blockchainFilesEnumerator;

        /// <summary>
        /// The "magic" ID of each Bitcoin block.
        /// </summary>
        private UInt32 blockMagicId = DefaultBlockMagicId;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockchainParser" /> class.
        /// </summary>
        /// <param name="blockchainPath">
        /// The path to the folder containing the blockchain files.
        /// </param>
        /// <exception cref="InvalidBlockchainFilesException">
        /// Thrown when the list of Bitcoin blockchain files is found to be invalid.
        /// The blockchain folder must contain files named with the pattern "blkxxxxx.dat", 
        /// starting from "blk00000.dat" and with no gaps in the numeric section.
        /// Note that this exception is referring only to the file names and not to the files content.
        /// </exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", Justification = "blk and dat refer to file names and extensions")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "blk and dat refer to file names and extensions.")]
        public BlockchainParser(string blockchainPath)
            : this(GetBlockchainFiles(GetFileInfoList(blockchainPath, null)))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockchainParser" /> class.
        /// </summary>
        /// <param name="blockchainPath">
        /// The path to the folder containing the blockchain files.
        /// </param>
        /// <param name="firstBlockchainFileName">
        /// The name of the first blockchain file that should be processed from the series of blockchain files. 
        /// In the list of blockchain files ordered by name, any blockchain file that appears prior
        /// to the file specified by this parameter will be ignored.
        /// If null then all file from the series of blockchain files will be processed.
        /// </param>
        /// <exception cref="InvalidBlockchainFilesException">
        /// Thrown when the list of Bitcoin blockchain files is found to be invalid.
        /// The blockchain folder must contain files named with the pattern "blkxxxxx.dat", 
        /// starting from "blk00000.dat" and with no gaps in the numeric section.
        /// Note that this exception is referring only to the file names and not to the files content.
        /// </exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", Justification = "blk and dat refer to file names and extensions")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "blk and dat refer to file names and extensions.")]
        public BlockchainParser(string blockchainPath, string firstBlockchainFileName)
            : this(GetBlockchainFiles(GetFileInfoList(blockchainPath, firstBlockchainFileName)))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockchainParser" /> class.
        /// </summary>
        /// <param name="blockchainFilesEnumerator">
        /// An enumerable providing access to a set of BlockchainFile instances, each representing a Bitcoin blockchain file.
        /// </param>
        public BlockchainParser(IEnumerable<BlockchainFile> blockchainFilesEnumerator)
        {
            this.blockchainFilesEnumerator = blockchainFilesEnumerator;
        }

        /// <summary>
        /// Sets the value that will be used to check against the BlockId of each block.
        /// If this method is not called then the default value of 0xD9B4BEF9 will be used.
        /// </summary>
        /// <param name="blockId">The value that will be used to check against the BlockId of each block.</param>
        public void SetBlockId(UInt32 blockId)
        {
            this.blockMagicId = blockId;
        }

        /// <summary>
        /// Parses the Bitcoin blockchain and returns a <see cref="IEnumerable&lt;Block&gt;"/>.
        /// Each element contains information about one Bitcoin block.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerable&lt;Block&gt;"/>.
        /// Each element contains information about one Bitcoin block.
        /// </returns>
        public IEnumerable<Block> ParseBlockchain()
        {
            foreach (BlockchainFile blockchainFile in this.blockchainFilesEnumerator)
            {
                foreach (Block block in this.ParseBlockchainFile(blockchainFile))
                {
                    yield return block;
                }
            }
        }

        /// <summary>
        /// Parses a Bitcoin block header.
        /// </summary>
        /// <param name="blockMemoryStreamReader">
        /// Provides access to a section of the Bitcoin blockchain file.
        /// </param>
        /// <returns>
        /// The block header information.
        /// </returns>
        /// <exception cref="InvalidBlockchainContentException">
        /// Thrown if the block version is unknown.
        /// </exception>
        private static BlockHeader ParseBlockHeader(BlockMemoryStreamReader blockMemoryStreamReader)
        {
            BlockHeader blockHeader = new BlockHeader();

            int positionInBaseStreamAtBlockHeaderStart = (int)blockMemoryStreamReader.BaseStream.Position;

            blockHeader.BlockVersion = blockMemoryStreamReader.ReadUInt32();

            //// TODO: We need to understand better what is different in V2 and V3.

            if (blockHeader.BlockVersion != 1 &&
                blockHeader.BlockVersion != 2 &&
                blockHeader.BlockVersion != 3 &&
                blockHeader.BlockVersion != 0x20000007 &&
                blockHeader.BlockVersion != 0x30000000 &&
                blockHeader.BlockVersion != 4 &&
                blockHeader.BlockVersion != 0x20000000 &&
                blockHeader.BlockVersion != 0x20000001 &&
                blockHeader.BlockVersion != 0x30000001 &&
                blockHeader.BlockVersion != 0x08000004 &&
                blockHeader.BlockVersion != 0x20000002 &&
                blockHeader.BlockVersion != 0x30000007 &&
                blockHeader.BlockVersion != 0x20000004)
            {
                throw new UnknownBlockVersionException(string.Format(CultureInfo.InvariantCulture, "Unknown block version: {0} ({0:X}).", blockHeader.BlockVersion));
            }

            blockHeader.PreviousBlockHash = new ByteArray(blockMemoryStreamReader.ReadBytes(32).ReverseByteArray());
            blockHeader.MerkleRootHash = new ByteArray(blockMemoryStreamReader.ReadBytes(32).ReverseByteArray());

            blockHeader.BlockTimestampUnix = blockMemoryStreamReader.ReadUInt32();
            blockHeader.BlockTimestamp = new DateTime(1970, 1, 1).AddSeconds(blockHeader.BlockTimestampUnix);

            blockHeader.BlockTargetDifficulty = blockMemoryStreamReader.ReadUInt32();
            blockHeader.BlockNonce = blockMemoryStreamReader.ReadUInt32();

            int positionInBaseStreamAfterBlockHeaderEnd = (int)blockMemoryStreamReader.BaseStream.Position;

            using (SHA256Managed sha256 = new SHA256Managed())
            {
                //// We need to calculate the double SHA256 hash of this transaction.
                //// We need to access the buffer that contains the transaction that we jut read through. 
                //// Here we take advantage of the fact that the entire block was loaded as an in-memory buffer.
                //// The base stream of blockMemoryStreamReader is that in-memory buffer.

                byte[] baseBuffer = blockMemoryStreamReader.GetBuffer();
                int blockHeaderBufferSize = positionInBaseStreamAfterBlockHeaderEnd - positionInBaseStreamAtBlockHeaderStart;

                if (blockHeaderBufferSize != ExpectedBlockHeaderBufferSize)
                {
                    // We have a problem. The block header should be 80 bytes in size.
                    throw new InvalidBlockchainContentException(string.Format(CultureInfo.InvariantCulture, "Block header buffer size has an invalid length: {0}. Expected: {1}.", blockHeaderBufferSize, ExpectedBlockHeaderBufferSize));
                }

                byte[] hash1 = sha256.ComputeHash(baseBuffer, positionInBaseStreamAtBlockHeaderStart, blockHeaderBufferSize);
                blockHeader.BlockHash = new ByteArray(sha256.ComputeHash(hash1).ReverseByteArray());
            }

            return blockHeader;
        }

        /// <summary>
        /// Parses a Bitcoin transaction input.
        /// </summary>
        /// <param name="blockMemoryStreamReader">
        /// Provides access to a section of the Bitcoin blockchain file.
        /// </param>
        /// <returns>
        /// The Bitcoin transaction input that was parsed.
        /// </returns>
        private static TransactionInput ParseTransactionInput(BlockMemoryStreamReader blockMemoryStreamReader)
        {
            TransactionInput transactionInput = new TransactionInput();

            transactionInput.SourceTransactionHash = new ByteArray(blockMemoryStreamReader.ReadBytes(32).ReverseByteArray());
            transactionInput.SourceTransactionOutputIndex = blockMemoryStreamReader.ReadUInt32();

            int scriptLength = (int)blockMemoryStreamReader.ReadVariableLengthInteger();

            // Ignore the script portion.
            transactionInput.InputScript = new ByteArray(blockMemoryStreamReader.ReadBytes(scriptLength));

            // Ignore the sequence number. 
            blockMemoryStreamReader.SkipBytes(4);

            return transactionInput;
        }

        /// <summary>
        /// Parses a Bitcoin transaction output.
        /// </summary>
        /// <param name="blockMemoryStreamReader">
        /// Provides access to a section of the Bitcoin blockchain file.
        /// </param>
        /// <returns>
        /// The Bitcoin transaction output that was parsed.
        /// </returns>
        private static TransactionOutput ParseTransactionOutput(BlockMemoryStreamReader blockMemoryStreamReader)
        {
            TransactionOutput transactionOutput = new TransactionOutput();

            transactionOutput.OutputValueSatoshi = blockMemoryStreamReader.ReadUInt64();
            int scriptLength = (int)blockMemoryStreamReader.ReadVariableLengthInteger();
            transactionOutput.OutputScript = new ByteArray(blockMemoryStreamReader.ReadBytes(scriptLength));

            return transactionOutput;
        }

        /// <summary>
        /// Parses a Bitcoin transaction.
        /// </summary>
        /// <param name="blockMemoryStreamReader">
        /// Provides access to a section of the Bitcoin blockchain file.
        /// </param>
        /// <returns>
        /// The Bitcoin transaction that was parsed.
        /// </returns>
        private static Transaction ParseTransaction(BlockMemoryStreamReader blockMemoryStreamReader)
        {
            Transaction transaction = new Transaction();

            int positionInBaseStreamAtTransactionStart = (int)blockMemoryStreamReader.BaseStream.Position;

            transaction.TransactionVersion = blockMemoryStreamReader.ReadUInt32();

            int inputsCount = (int)blockMemoryStreamReader.ReadVariableLengthInteger();

            for (int inputIndex = 0; inputIndex < inputsCount; inputIndex++)
            {
                TransactionInput transactionInput = BlockchainParser.ParseTransactionInput(blockMemoryStreamReader);
                transaction.AddInput(transactionInput);
            }

            int outputsCount = (int)blockMemoryStreamReader.ReadVariableLengthInteger();

            for (int outputIndex = 0; outputIndex < outputsCount; outputIndex++)
            {
                TransactionOutput transactionOutput = BlockchainParser.ParseTransactionOutput(blockMemoryStreamReader);
                transaction.AddOutput(transactionOutput);
            }

            // TODO: Need to find out more details about the semantic of TransactionLockTime.
            transaction.TransactionLockTime = blockMemoryStreamReader.ReadUInt32();

            int positionInBaseStreamAfterTransactionEnd = (int)blockMemoryStreamReader.BaseStream.Position;

            using (SHA256Managed sha256 = new SHA256Managed())
            {
                //// We need to calculate the double SHA256 hash of this transaction.
                //// We need to access the buffer that contains the transaction that we jut read through. 
                //// Here we take advantage of the fact that the entire block was loaded as an in-memory buffer.
                //// The base stream of blockMemoryStreamReader is that in-memory buffer.

                byte[] baseBuffer = blockMemoryStreamReader.GetBuffer();
                int transactionBufferSize = positionInBaseStreamAfterTransactionEnd - positionInBaseStreamAtTransactionStart;

                byte[] hash1 = sha256.ComputeHash(baseBuffer, positionInBaseStreamAtTransactionStart, transactionBufferSize);
                transaction.TransactionHash = new ByteArray(sha256.ComputeHash(hash1).ReverseByteArray());
            }

            return transaction;
        }

        /// <summary>
        /// Parses one Bitcoin block except for a few fields before the actual block header.
        /// </summary>
        /// <param name="blockchainFileName">
        /// The name of the blockchain file that contains the block being parsed.
        /// </param>
        /// <param name="blockMemoryStreamReader">
        /// Provides access to a section of the Bitcoin blockchain file.
        /// </param>
        private static Block InternalParseBlockchainFile(string blockchainFileName, BlockMemoryStreamReader blockMemoryStreamReader)
        {
            BlockHeader blockHeader = BlockchainParser.ParseBlockHeader(blockMemoryStreamReader);

            Block block = new Block(blockchainFileName, blockHeader);

            int blockTransactionCount = (int)blockMemoryStreamReader.ReadVariableLengthInteger();

            for (int transactionIndex = 0; transactionIndex < blockTransactionCount; transactionIndex++)
            {
                Transaction transaction = BlockchainParser.ParseTransaction(blockMemoryStreamReader);
                block.AddTransaction(transaction);
            }

            return block;
        }

        /// <summary>
        /// Retrieves a list ordered by name of FileInfo instances representing all blockchain files that will be processed.
        /// </summary>
        /// <param name="blockchainPath">
        /// The path to the folder containing the blockchain files.
        /// </param>
        /// <param name="firstBlockchainFileName">
        /// The name of the first blockchain file that should be processed from the series of blockchain files.
        /// In the list of blockchain files ordered by name, any blockchain file that appears prior
        /// to the file specified by this parameter will be ignored.
        /// </param>
        /// <returns>
        /// A list ordered by name of FileInfo instances representing all blockchain files that will be processed.
        /// </returns>
        private static List<FileInfo> GetFileInfoList(string blockchainPath, string firstBlockchainFileName)
        {
            List<FileInfo> blockchainFiles = GetFileInfoList(blockchainPath);
            ValidateBlockchainFiles(blockchainFiles, firstBlockchainFileName);

            return SelectFilesToProcess(blockchainFiles, firstBlockchainFileName);
        }

        /// <summary>
        /// Retrieves a list ordered by name of FileInfo instances representing all blockchain files in the given path.
        /// </summary>
        /// <param name="blockchainPath">
        /// The path to the folder containing the blockchain files.
        /// </param>
        /// <returns>
        /// A list ordered by name of FileInfo instances representing all blockchain files in the given path.
        /// </returns>
        private static List<FileInfo> GetFileInfoList(string blockchainPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(blockchainPath);
            return directoryInfo.GetFiles(BlockFileSelector, SearchOption.TopDirectoryOnly).OrderBy(f => f.Name).ToList();
        }

        /// <summary>
        /// Validates the given list of file names.
        /// </summary>
        /// <param name="blockchainFiles">
        /// A list ordered by name of FileInfo instances representing all blockchain files in the given path.
        /// </param>
        /// <param name="firstBlockchainFileName">
        /// The name of the first blockchain file that should be processed from the series of blockchain files. 
        /// </param>
        /// <exception cref="InvalidBlockchainFilesException">
        /// Thrown when the list of Bitcoin blockchain files is found to be invalid.
        /// The blockchain folder must contain files named with the pattern "blkxxxxx.dat", 
        /// starting from "blk00000.dat" and with no gaps in the numeric section.
        /// Note that this exception is referring only to the file names and not to the files content.
        /// </exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", Justification = "blk and dat refer to file names and extensions")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "blk and dat refer to file names and extensions.")]
        private static void ValidateBlockchainFiles(List<FileInfo> blockchainFiles, string firstBlockchainFileName)
        {
            bool lastKnownBlockchainFileFound = false;

            for (int i = 0; i < blockchainFiles.Count; i++)
            {
                string expectedFileName = string.Format(CultureInfo.InvariantCulture, "blk{0:00000}.dat", i);
                if (expectedFileName != blockchainFiles[i].Name)
                {
                    throw new InvalidBlockchainFilesException("The blockchain folder must contain files named with the pattern \"blk?????.dat\", starting from \"blk00000.dat\" and with no gaps in the numeric section.");
                }

                if (firstBlockchainFileName == blockchainFiles[i].Name)
                {
                    lastKnownBlockchainFileFound = true;
                }
            }

            if (firstBlockchainFileName != null && lastKnownBlockchainFileFound == false)
            {
                throw new InvalidBlockchainFilesException(string.Format(CultureInfo.CurrentCulture, "The blockchain folder must contain file {0}.", firstBlockchainFileName));
            }
        }

        /// <summary>
        /// Retrieves the list of blockchain files that will have to be processed based on the list
        /// of all blockchain files and the name of the blockchain file that must be processed first. 
        /// </summary>
        /// <param name="allBlockchainFiles">
        /// The list ordered by name of all blockchain files found at the blockchain folder.
        /// </param>
        /// <param name="firstBlockchainFileName">
        /// The name of the first blockchain file that should be processed from the series of blockchain files.
        /// In the list of blockchain files ordered by name, any blockchain file that appears prior
        /// to the file specified by this parameter will be ignored.
        /// </param>
        /// <returns>
        /// The list ordered by name of all blockchain files that should be processed. 
        /// In the list of blockchain files given any blockchain files 
        /// that appear prior to the file specified by firstBlockchainFileName will be ignored.
        /// </returns>
        private static List<FileInfo> SelectFilesToProcess(IEnumerable<FileInfo> allBlockchainFiles, string firstBlockchainFileName)
        {
            List<FileInfo> fileInfoList = new List<FileInfo>();

            bool processFiles = firstBlockchainFileName == null;

            foreach (FileInfo fileInfo in allBlockchainFiles)
            {
                if (processFiles == false && firstBlockchainFileName == fileInfo.Name)
                {
                    // We found the last blockchain file we processed in the previous session.
                    // We can now start to process blockchain files.
                    processFiles = true;
                }

                if (processFiles)
                {
                    fileInfoList.Add(fileInfo);
                }
            }

            return fileInfoList;
        }

        /// <summary>
        /// Transforms an enumerable of instances of type <see cref="BlockchainFile"/> into an enumerable of type <see cref="BlockchainFile "/>.
        /// </summary>
        /// <param name="fileInfoList">
        /// A list of files specifying all Bitcoin blockchain files that have to be converted in instances of type <see cref="BlockchainFile"/> class.
        /// </param>
        /// <returns>
        /// An enumerable providing access to a set of <see cref="BlockchainFile "/> instances, each representing a Bitcoin blockchain file.
        /// </returns>
        private static IEnumerable<BlockchainFile> GetBlockchainFiles(IEnumerable<FileInfo> fileInfoList)
        {
            foreach (FileInfo fileInfo in fileInfoList)
            {
                using (FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader binaryReader = new BinaryReader(fileStream))
                    {
                        yield return new BlockchainFile(fileInfo.Name, binaryReader);
                    }
                }
            }
        }

        /// <summary>
        /// Parses one Bitcoin block.
        /// </summary>
        /// <param name="blockchainFileName">
        /// The name of the blockchain file that contains the block being parsed.
        /// </param>
        /// <param name="binaryReader">
        /// Provides access to a Bitcoin blockchain file.
        /// </param>
        private Block ParseBlockchainFile(string blockchainFileName, BinaryReader binaryReader)
        {
            // There are some rare situations where a block is preceded by a section containing zero bytes. 
            if (binaryReader.SkipZeroBytes() == false)
            {
                // We reached the end of the file. There is no block to be parsed.
                return null;
            }

            UInt32 blockId = binaryReader.ReadUInt32();
            if (blockId != this.blockMagicId)
            {
                throw new InvalidBlockchainContentException(string.Format(CultureInfo.InvariantCulture, "Invalid block Id: {0:X}. Expected: {1:X}", blockId, this.blockMagicId));
            }

            int blockLength = (int)binaryReader.ReadUInt32();
            byte[] blockBuffer = binaryReader.ReadBytes(blockLength);

            using (BlockMemoryStreamReader blockMemoryStreamReader = new BlockMemoryStreamReader(blockBuffer))
            {
                return BlockchainParser.InternalParseBlockchainFile(blockchainFileName, blockMemoryStreamReader);
            }
        }

        /// <summary>
        /// Parses one Bitcoin blockchain file.
        /// </summary>
        /// <param name="blockchainFile">
        /// Contains information about and provides access to the Bitcoin blockchain file that needs to be parsed.
        /// </param>
        /// <returns>
        /// An IEnumerable containing instances of class <see cref="Block"/> each storing data about one Bitcoin block.
        /// </returns>
        private IEnumerable<Block> ParseBlockchainFile(BlockchainFile blockchainFile)
        {
            BinaryReader binaryReader = blockchainFile.BinaryReader;

            while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
            {
                Block block = this.ParseBlockchainFile(blockchainFile.FileName, binaryReader);
                if (block != null)
                {
                    block.PercentageOfCurrentBlockchainFile = (int)(100 * binaryReader.BaseStream.Position / binaryReader.BaseStream.Length);
                    yield return block;
                }
            }
        }
    }
}
