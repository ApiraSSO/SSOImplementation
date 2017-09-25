using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Kernel.Cryptography
{
    /// <summary>
	/// Abstract class for hashing. Provides static method to hash and verify the password
	/// </summary>
	public abstract class HashBase
    {
        protected abstract HashAlgorithm HashAlgorithm { get; }

        /// <summary>
        /// Generages a random salf value with default valur 4 bytes
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] GenerateSaltValue(uint length = 4)
        {
            var randBytes = new byte[length];

            using (var rand = new RNGCryptoServiceProvider())
            {

                rand.GetBytes(randBytes);
            }

            return randBytes;
        }

        #region Static Methods

        /// <summary>
        /// Generates a hash from plain text input. The method generates a salt and pass it to the caller
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="clearData"></param>
        /// <param name="saltValue"></param>
        /// <returns></returns>
        public static byte[] HashPassword(HashAlgorithm hash, string clearData, out byte[] saltValue)
        {
            saltValue = GenerateSaltValue();

            return HashPassword(hash, clearData, saltValue);
        }

        /// <summary>
        /// Generates a hash from secure string input. The method generates a salt and pass it to the caller
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="clearData"></param>
        /// <param name="saltValue"></param>
        /// <returns></returns>
        public static byte[] HashPassword(HashAlgorithm hash, SecureString clearData, out byte[] saltValue)
        {
            saltValue = GenerateSaltValue();

            return HashPassword(hash, clearData, saltValue);
        }

        /// <summary>
        /// Generates a hash from plain text input with a salt value provided by the caller
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="clearData"></param>
        /// <param name="saltValue"></param>
        /// <returns></returns>
        public static byte[] HashPassword(HashAlgorithm hash, string clearData, byte[] saltValue)
        {
            if (hash == null)
                throw new ArgumentNullException("HashAlgorithm");

            if (saltValue == null)
                throw new ArgumentNullException("Salt");

            UnicodeEncoding encoding = new UnicodeEncoding();

            if (clearData == null || encoding == null)
                return null;

            byte[] valueToHash = new byte[saltValue.Length + encoding.GetByteCount(clearData)];

            byte[] binaryPassword = encoding.GetBytes(clearData);

            saltValue.CopyTo(valueToHash, 0);

            binaryPassword.CopyTo(valueToHash, saltValue.Length);

            byte[] hashedPassword = hash.ComputeHash(valueToHash);

            return hashedPassword;
        }

        /// <summary>
        /// Generates a hash from secure string input with a salt value provided by the caller
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="secureString"></param>
        /// <param name="saltValue"></param>
        /// <returns></returns>
        public static byte[] HashPassword(HashAlgorithm hash, SecureString secureString, byte[] saltValue)
        {
            if (hash == null)
                throw new ArgumentNullException("HashAlgorithm");

            if (saltValue == null)
                throw new ArgumentNullException("Salt");

            if (secureString == null)
                return null;

            IntPtr unmanagedRef = IntPtr.Zero;

            try
            {
                unmanagedRef = Marshal.SecureStringToGlobalAllocAnsi(secureString);

                var chars = new List<byte>();

                for (var i = 0; i < secureString.Length; i++)
                {
                    var b = Marshal.ReadByte(unmanagedRef, i);

                    chars.Add(b);
                }


                var clearData = chars.ToArray();

                byte[] valueToHash = new byte[saltValue.Length + clearData.Length];

                saltValue.CopyTo(valueToHash, 0);

                clearData.CopyTo(valueToHash, saltValue.Length);

                byte[] hashedPassword = hash.ComputeHash(valueToHash);

                return hashedPassword;
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocAnsi(unmanagedRef);
            }
        }

        /// <summary>
        /// Verify password provided as a plain text
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="clearData"></param>
        /// <param name="hashedPassword"></param>
        /// <param name="saltValue"></param>
        /// <returns></returns>
        public static bool VerifyPassword(HashAlgorithm hash, string clearData, byte[] hashedPassword, byte[] saltValue)
        {
            var passwordToVerify = HashPassword(hash, clearData, saltValue);

            return CompareArrays(hashedPassword, passwordToVerify);
        }

        /// <summary>
        ///  Verify password provided as a secure string
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="clearData"></param>
        /// <param name="hashedPassword"></param>
        /// <param name="saltValue"></param>
        /// <returns></returns>
        public static bool VerifyPassword(HashAlgorithm hash, SecureString clearData, byte[] hashedPassword, byte[] saltValue)
        {
            var passwordToVerify = HashPassword(hash, clearData, saltValue);

            return CompareArrays(hashedPassword, passwordToVerify);
        }

        #endregion

        //#region Instance Method

        //public virtual byte[] HashPassword(string clearData, byte[] salt)
        //{
        //	return HashPassword(HashAlgorithm, clearData, salt);
        //}

        //public virtual byte[] HashPassword(string clearData, out byte[] salt)
        //{
        //	return HashPassword(HashAlgorithm, clearData, out salt);
        //}

        //public virtual byte[] HashPassword(SecureString clearData, out byte[] salt)
        //{
        //	return HashPassword(HashAlgorithm, clearData, out salt);
        //}

        //public virtual byte[] HashPassword(string clearData, string saltValue)
        //{
        //	throw new NotImplementedException();
        //}

        //public virtual bool VerifyPassword(string clearData, byte[] hashedPassword, byte[] saltValue)
        //{
        //	return VerifyPassword(HashAlgorithm, clearData, hashedPassword, saltValue);
        //}

        //public virtual bool VerifyPassword(SecureString clearData, byte[] hashedPassword, byte[] saltValue)
        //{
        //	return VerifyPassword(HashAlgorithm, clearData, hashedPassword, saltValue);
        //}

        //#endregion

        /// <summary>
        /// Compare two arrays for equality
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        private static bool CompareArrays(byte[] array1, byte[] array2)
        {
            if (array1 == null ^ array2 == null)
                return false;

            if (array1 == null && array2 == null)
                return false;

            return array1.SequenceEqual(array2);
        }

        /// <summary>
        /// Dispose managed resources
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (HashAlgorithm != null)
                HashAlgorithm.Dispose();
        }
    }
}
