//-----------------------------------------------------------------------
// <copyright file="ByteArray.cs">
// Copyright (c) Ladislau Molnar. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BitcoinBlockchain.Data
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// An immutable class, wrapper to a byte array.
    /// Because ByteArray implements GetHashCode and Equals based on the content of the byte array that is being wrapped, 
    /// it can be used as a key in a dictionary. 
    /// Note that the C# Array do not overwrite GetHashCode. Two byte arrays with identical content will 
    /// generate different values for GetHashCode. That prevents a C# byte array from being used directly 
    /// as a key in a dictionary. ByteArray can be used in that situation.
    /// </summary>
    [type: CLSCompliant(true)]
    public class ByteArray
    {
        /// <summary>
        /// A read-only instance of the ByteArray class whose byte array has zero length.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "The type ByteArray is immutable.")]
        public static readonly ByteArray Empty = new ByteArray(new byte[0]);

        /// <summary>
        /// The byte array that is wrapped by this instance.
        /// </summary>
        private readonly byte[] byteArrayValue;

        /// <summary>
        /// Indicates whether the hash code was calculated yet or not.
        /// </summary>
        private bool isHashCodeCalculated;

        /// <summary>
        /// Cached value for the result of GetHashCode. 
        /// Having this allows us to avoid recalculating the hash code every time GetHashCode is called.
        /// Note: initially we used a Lazy instance but the memory footprint for Lazy is too large so we 
        ///       eventually replaced this with a simple integer and a boolean flag: isHashCodeCalculated.
        /// </summary>
        private int hashCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteArray"/> class.
        /// </summary>
        /// <param name="sourceByteArray">
        /// The source byte array that will be wrapped by this <see cref="ByteArray"/> instance.
        /// </param>
        public ByteArray(byte[] sourceByteArray)
        {
            if (sourceByteArray == null)
            {
                throw new ArgumentNullException("sourceByteArray");
            }

            this.byteArrayValue = new byte[sourceByteArray.Length];
            Buffer.BlockCopy(sourceByteArray, 0, this.byteArrayValue, 0, sourceByteArray.Length);

            this.isHashCodeCalculated = false;
        }

        /// <summary>
        /// Implements the "==" operator for class ByteArray.
        /// </summary>
        /// <param name="byteArray1">
        /// An instance of type<see cref="ByteArray" />, the left operand of the comparison.
        /// </param>
        /// <param name="byteArray2">
        /// An instance of type<see cref="ByteArray" />, the right operand of the comparison.
        /// </param>
        /// <returns>
        /// True if the two instances of ByteArray store identical byte arrays; false otherwise.
        /// </returns>
        public static bool operator ==(ByteArray byteArray1, ByteArray byteArray2)
        {
            // If both are null, or both are same instance, return true.
            if (object.ReferenceEquals(byteArray1, byteArray2))
            {
                return true;
            }

            if ((object)byteArray1 == null || (object)byteArray2 == null)
            {
                return false;
            }

            if (byteArray1.byteArrayValue.Length != byteArray2.byteArrayValue.Length)
            {
                return false;
            }

            for (int i = 0; i < byteArray1.byteArrayValue.Length; i++)
            {
                if (byteArray1.byteArrayValue[i] != byteArray2.byteArrayValue[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Implements the "!=" operator for class ByteArray.
        /// </summary>
        /// <param name="byteArray1">
        /// An instance of type<see cref="ByteArray" />, the left operand of the comparison.
        /// </param>
        /// <param name="byteArray2">
        /// An instance of type<see cref="ByteArray" />, the right operand of the comparison.
        /// </param>
        /// <returns>
        /// False if the two instances of ByteArray store identical byte arrays; true otherwise.
        /// </returns>
        public static bool operator !=(ByteArray byteArray1, ByteArray byteArray2)
        {
            return (byteArray1 == byteArray2) == false;
        }

        /// <summary>
        /// Determines if all the bytes in the byte array have value 0.
        /// </summary>
        /// <returns>
        /// True if all the bytes in the byte array have value 0; false otherwise.
        /// </returns>
        public bool IsZeroArray()
        {
            return this.byteArrayValue.All(b => b == 0);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the "this" instance.
        /// </summary>
        /// <param name="obj">
        /// The object to compare with the "this" instance.
        /// </param>
        /// <returns>
        /// true if the specified object is equal to the "this" instance; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            ByteArray byteArray = obj as ByteArray;
            if (byteArray == null)
            {
                return false;
            }

            return this == byteArray;
        }

        // ReSharper disable NonReadonlyFieldInGetHashCode

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// The hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            if (this.isHashCodeCalculated == false)
            {
                this.hashCode = this.CalculateHashCode();
                this.isHashCodeCalculated = true;
            }

            return this.hashCode;
        }

        /// <summary>
        /// Returns a copy of the byte array that is being wrapped by this instance.
        /// </summary>
        /// <remarks>
        /// We return a copy instead of the byte array field stored by this instance in order to ensure the immutability of this class.
        /// </remarks>>
        /// <returns>
        /// A copy of the byte array that is being wrapped by this instance.
        /// </returns>
        public byte[] ToArray()
        {
            byte[] byteArrayCopy = new byte[this.byteArrayValue.Length];
            Buffer.BlockCopy(this.byteArrayValue, 0, byteArrayCopy, 0, this.byteArrayValue.Length);

            return byteArrayCopy;
        }

        /// <summary>
        /// Converts the numeric value of each element of the given 
        /// byte array to its equivalent hexadecimal string representation.
        /// </summary>
        /// <returns>
        /// A string of hexadecimal pairs, where each pair represents
        /// a byte in the given byte array. For example, "00FF00AB".
        /// </returns>
        public override string ToString()
        {
            return BitConverter.ToString(this.byteArrayValue).Replace("-", string.Empty);
        }

        /// <summary>
        /// Calculates the value for <c>GetHashCode()</c>.
        /// </summary>
        /// <returns>
        /// The value for <c>GetHashCode()</c>.
        /// </returns>
        private int CalculateHashCode()
        {
            unchecked
            {
                int hashCodeResult = 17;
                for (int i = 0; i < this.byteArrayValue.Length; i++)
                {
                    hashCodeResult = (hashCodeResult * 23) + this.byteArrayValue[i];
                }

                return hashCodeResult;
            }
        }
    }
}
