//-----------------------------------------------------------------------
// <copyright file="Witness.cs">
// Copyright © Jeffrey Quesnelle. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BitcoinBlockchain.Data
{
    using System.Collections.Generic;

    /// <summary>
    /// Contains witness data for transaction inputs. Currently the witness data itself is unparsed
    /// For more information see:
    /// <c>https://bitcoincore.org/en/segwit_wallet_dev/</c>
    /// </summary>
    public class Witness
    {
        /// <summary>
        /// Witness data stack
        /// </summary>
        public List<ByteArray> WitnessStack { get; set; }
    }
}
