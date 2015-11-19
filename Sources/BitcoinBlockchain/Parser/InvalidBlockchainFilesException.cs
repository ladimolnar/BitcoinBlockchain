//-----------------------------------------------------------------------
// <copyright file="InvalidBlockchainFilesException.cs">
// Copyright © Ladislau Molnar. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BitcoinBlockchain.Parser
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    /// <summary>
    /// The exception that is thrown when the list of Bitcoin blockchain files is found to be invalid.
    /// The blockchain folder must contain files named with the pattern "blkxxxxx.dat", 
    /// starting from "blk00000.dat" and with no gaps in the numeric section.
    /// Note that this exception is referring only to the file names and not to the files content.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", Justification = "blk and dat refer to file names and extensions")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "blk and dat refer to file names and extensions.")]
    [Serializable]
    public class InvalidBlockchainFilesException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidBlockchainFilesException"/> class.
        /// </summary>
        public InvalidBlockchainFilesException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidBlockchainFilesException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error.
        /// </param>
        public InvalidBlockchainFilesException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidBlockchainFilesException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception,
        /// or a null reference if no inner exception is specified.
        /// </param>
        public InvalidBlockchainFilesException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidBlockchainFilesException"/> class with serialized data.
        /// </summary>
        /// <param name="info">
        /// The System.Runtime.Serialization.SerializationInfo that holds the serialized data 
        /// about this instance of the <see cref="InvalidBlockchainFilesException"/> class.
        /// </param>
        /// <param name="context">
        /// The System.Runtime.Serialization.StreamingContext that contains contextual information about the 
        /// source or destination.
        /// </param>
        protected InvalidBlockchainFilesException(
           SerializationInfo info,
           StreamingContext context)
            : base(info, context)
        {
        }
    }
}
