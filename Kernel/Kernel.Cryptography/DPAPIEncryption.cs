using System;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Kernel.Extensions;

namespace Kernel.Cryptography
{
    /// <summary>
	/// Encrypt and decrypt a string using local machine key 
	/// </summary>
	public class DPAPIEncryption
    {
        /// <summary>
        /// Encrypts the string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="entropy">The entropy.</param>
        /// <returns></returns>
        public static string EncryptString(string input, byte[] entropy = null)
        {
            var securedString = StringExtensions.ToSecureString(input);
            input = null;
            return DPAPIEncryption.EncryptString(securedString, entropy);
        }

        /// <summary>
        /// Encrypts the string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="entropy">The entropy.</param>
        /// <returns></returns>
        public static string EncryptString(SecureString input, byte[] entropy = null)
        {
            byte[] encryptedData = ProtectedData.Protect(
                Encoding.Unicode.GetBytes(StringExtensions.ToInsecureString(input)),
                entropy,
                DataProtectionScope.LocalMachine);

            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// Decrypts the string.
        /// </summary>
        /// <param name="encryptedData">The encrypted data.</param>
        /// <param name="entropy">The entropy.</param>
        /// <returns></returns>
        public static SecureString DecryptString(string encryptedData, byte[] entropy = null)
        {
            try
            {
                byte[] decryptedData = ProtectedData.Unprotect(
                    Convert.FromBase64String(encryptedData),
                    entropy,
                    DataProtectionScope.LocalMachine);

                return StringExtensions.ToSecureString(Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new SecureString();
            }
        }
    }
}
