//-----------------------------------------------------------------------
// <copyright file="BlockMemoryStreamReader.cs">
// Copyright © Ladislau Molnar. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BitcoinBlockchain.Parser
{
    using System;
    using System.IO;

    /// <summary>
    /// This class provides low level functionality used to read the content of a byte array that stores sections of a Bitcoin blockchain file.
    /// </summary>
    internal class BlockMemoryStreamReader : IDisposable
    {
        /// <summary>
        /// The original byte array that is being wrapped by this instance of <see cref="BlockMemoryStreamReader"/> class.
        /// </summary>
        private readonly byte[] byteArray;

        /// <summary>
        /// The underlying memory stream created on top of the <c>byteArray</c> field.
        /// </summary>
        private readonly MemoryStream memoryStream;

        /// <summary>
        /// The underlying binary reader created on top of the <c>byteArray</c> field.
        /// </summary>
        private readonly BinaryReader binaryReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockMemoryStreamReader" /> class.
        /// An instance created with this constructor is a wrapper over a byte array 
        /// instance that provides access to section of a Bitcoin blockchain file.
        /// </summary>
        /// <param name="byteArray">
        /// A byte array that provides access to section of a Bitcoin blockchain file.
        /// </param>
        public BlockMemoryStreamReader(byte[] byteArray)
        {
            this.byteArray = byteArray;
            this.memoryStream = new MemoryStream(byteArray);
            this.binaryReader = new BinaryReader(this.memoryStream);
        }

        /// <summary>
        /// Gets the stream that is used by this instance.
        /// </summary>
        public Stream BaseStream
        {
            get { return this.binaryReader.BaseStream; }
        }

        /// <summary>
        /// Implements the IDisposable pattern.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Returns the underlying byte array.
        /// </summary>
        /// <returns>The underlying byte array.</returns>
        public byte[] GetBuffer()
        {
            return this.byteArray;
        }

        /// <summary>
        /// Wrapper over <c>BinaryReader.ReadByte</c>.
        /// </summary>
        public byte ReadByte()
        {
            return this.binaryReader.ReadByte();
        }

        /// <summary>
        /// Wrapper over <c>BinaryReader.ReadBytes</c>.
        /// </summary>
        /// <param name="count">
        /// The number of bytes to read.
        /// </param>
        public byte[] ReadBytes(int count)
        {
            return this.binaryReader.ReadBytes(count);
        }

        /// <summary>
        /// Wrapper over <c>binaryReader.ReadUInt16</c>
        /// </summary>
        public UInt16 ReadUInt16()
        {
            return this.binaryReader.ReadUInt16();
        }

        /// <summary>
        /// Wrapper over <c>binaryReader.ReadUInt32</c>
        /// </summary>
        public UInt32 ReadUInt32()
        {
            return this.binaryReader.ReadUInt32();
        }

        /// <summary>
        /// Wrapper over <c>binaryReader.ReadUInt64</c>
        /// </summary>
        public UInt64 ReadUInt64()
        {
            return this.binaryReader.ReadUInt64();
        }

        /// <summary>
        /// Advances the position of the stream by the given number of bytes 
        /// or until the end of the stream is reached.
        /// </summary>
        /// <param name="count">
        /// The number of bytes to skip.
        /// </param>
        public void SkipBytes(int count)
        {
            this.binaryReader.ReadBytes(count);
        }

        /// <summary>
        /// Reads a variable length integer.
        /// See: https://en.bitcoin.it/wiki/Protocol_specification#Variable_length_integer
        /// </summary>
        public UInt64 ReadVariableLengthInteger()
        {
            byte oneByte = this.ReadByte();
            if (oneByte < 0xFD)
            {
                return oneByte;
            }
            else
            {
                UInt16 twoBytes = this.ReadUInt16();
                if (twoBytes < 0xFFFF)
                {
                    return twoBytes;
                }
                else
                {
                    UInt32 fourBytes = this.ReadUInt32();
                    if (fourBytes < 0xFFFFFFFF)
                    {
                        return fourBytes;
                    }
                    else
                    {
                        UInt64 eightBytes = this.ReadUInt64();
                        return eightBytes;
                    }
                }
            }
        }

        /// <summary>
        /// Implements the IDisposable pattern.
        /// </summary>
        /// <param name="disposing">
        /// True to perform cleanup, false otherwise.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.binaryReader.Dispose();
                this.memoryStream.Dispose();
            }
        }
    }
}
