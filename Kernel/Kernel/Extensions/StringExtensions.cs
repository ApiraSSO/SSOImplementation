using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Kernel.Extensions
{
	/// <summary>
	/// String extensions
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Convert the string to secure string
		/// </summary>
		/// <param name="input">Insecure string</param>
		/// <returns>Instance of secure string representing insecure string</returns>
		public static SecureString ToSecureString(this string input)
		{
			var secureString = new SecureString();

			if (input == null)
				return secureString;

			foreach (char c in input)
			{
				secureString.AppendChar(c);
			}

			secureString.MakeReadOnly();

			return secureString;
		}

		/// <summary>
		/// To the insecure string.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">secure string</exception>
		public static string ToInsecureString(this SecureString input)
		{
			if (input == null)
				throw new ArgumentNullException("secure string");

			var returnValue = string.Empty;

			IntPtr ptr = Marshal.SecureStringToBSTR(input);

			try
			{
				returnValue = Marshal.PtrToStringBSTR(ptr);
			}
			finally
			{
				Marshal.ZeroFreeBSTR(ptr);
			}

			return returnValue;
		}
	}
}