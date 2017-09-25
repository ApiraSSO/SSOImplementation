namespace Kernel.Configuration
{
	using System;
	using System.Security;
	using Kernel.Configuration;
	using Kernel.Extensions;
	using Kernel.Reflection;

	/// <summary>
	/// Provides application settings
	/// </summary>
	public class ApplicationSettingProvider
	{
		private static string _applicationName;

		private static string _dataStorePrefix;

		private static SecureString _password;

		private static string _mailAddressFrom;

		/// <summary>
		/// Gets the name of the application.
		/// </summary>
		/// <value>
		/// The name of the application.
		/// </value>
		public static string ApplicationName
		{
			get
			{
				if (_applicationName == null)
					_applicationName = AppSettingsConfigurationManager.GetSetting("applicationName", "AssetManagmentApplication");

				return _applicationName;
			}
		}

		/// <summary>
		/// Gets the data store prefix.
		/// </summary>
		/// <value>
		/// The data store prefix.
		/// </value>
		public static string DataStorePrefix
		{
			get
			{
				if (_dataStorePrefix == null)
					_dataStorePrefix = AppSettingsConfigurationManager.GetSetting("dataStorePrefix");

				return _dataStorePrefix;
			}
		}

		/// <summary>
		/// Gets the name of the unity master database.
		/// </summary>
		/// <value>
		/// The name of the unity master database.
		/// </value>
		public static string UnityMasterDatabaseName
		{
			get
			{
				return string.Format("{0}Master", DataStorePrefix);
			}
		}

		/// <summary>
		/// Gets the mail address from.
		/// </summary>
		/// <value>
		/// The mail address from.
		/// </value>
		public static string MailAddressFrom
		{
			get
			{
				if (_mailAddressFrom == null)
					_mailAddressFrom = AppSettingsConfigurationManager.GetSetting("mailAddressFrom");

				return _mailAddressFrom;
			}
		}

		/// <summary>
		/// Gets the password.
		/// </summary>
		/// <value>
		/// The password.
		/// </value>
		public static SecureString Password
		{
			get
			{
				if (_password == null)
#if DEBUG
					_password = StringExtensions.ToSecureString("Password1");
#else
                    _password = DPAPIEncryption.DecryptString(AppSettingsConfigurationManager.GetSetting("Password", ""));
#endif

				return _password;
			}
		}

		/// <summary>
		/// Ensures the valid setting.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <param name="func">The function.</param>
		/// <returns></returns>
		public static bool EnsureValidSetting<T>(string value, Func<string, bool> func = null)
		{
			if (func == null && typeof(T).IsValueType)
			{
				T result;

				ReflectionHelper.TryParseOrDefault<T>(value, out result);
			}

			return false;
		}
	}
}