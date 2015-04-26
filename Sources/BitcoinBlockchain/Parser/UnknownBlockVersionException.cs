//-----------------------------------------------------------------------
// <copyright file="UnknownBlockVersionException.cs">
// Copyright © Ladislau Molnar. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BitcoinBlockchain.Parser
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The exception that is thrown when the Bitcoin blockchain contains blocks with a version that cannot be processed.
    /// </summary>
    [Serializable]
    public class UnknownBlockVersionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownBlockVersionException"/> class.
        /// </summary>
        public UnknownBlockVersionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownBlockVersionException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error.
        /// </param>
        public UnknownBlockVersionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownBlockVersionException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception,
        /// or a null reference if no inner exception is specified.
        /// </param>
        public UnknownBlockVersionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownBlockVersionException"/> class with serialized data.
        /// </summary>
        /// <param name="info">
        /// The System.Runtime.Serialization.SerializationInfo that holds the serialized data 
        /// about this instance of the <see cref="UnknownBlockVersionException"/> class.
        /// </param>
        /// <param name="context">
        /// The System.Runtime.Serialization.StreamingContext that contains contextual information about the 
        /// source or destination.
        /// </param>
        protected UnknownBlockVersionException(
           SerializationInfo info,
           StreamingContext context)
            : base(info, context)
        {
        }
    }
}
