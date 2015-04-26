//-----------------------------------------------------------------------
// <copyright file="InvalidBlockchainContentException.cs">
// Copyright © Ladislau Molnar. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BitcoinBlockchain.Parser
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The exception that is thrown when the content of the Bitcoin blockchain files is found to be invalid.
    /// </summary>
    [Serializable]
    public class InvalidBlockchainContentException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidBlockchainContentException"/> class.
        /// </summary>
        public InvalidBlockchainContentException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidBlockchainContentException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error.
        /// </param>
        public InvalidBlockchainContentException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidBlockchainContentException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception,
        /// or a null reference if no inner exception is specified.
        /// </param>
        public InvalidBlockchainContentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidBlockchainContentException"/> class with serialized data.
        /// </summary>
        /// <param name="info">
        /// The System.Runtime.Serialization.SerializationInfo that holds the serialized data 
        /// about this instance of the <see cref="InvalidBlockchainContentException"/> class.
        /// </param>
        /// <param name="context">
        /// The System.Runtime.Serialization.StreamingContext that contains contextual information about the 
        /// source or destination.
        /// </param>
        protected InvalidBlockchainContentException(
           SerializationInfo info,
           StreamingContext context)
            : base(info, context)
        {
        }
    }
}
