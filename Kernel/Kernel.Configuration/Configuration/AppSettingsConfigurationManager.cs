namespace Kernel.Configuration
{
	using System;
	using System.Configuration;
	using Kernel.Reflection;

    public class AppSettingsConfigurationManager
    {
        public static string GetSetting(string key, string defaultValue = null)
        {
            var settingValue = ConfigurationManager.AppSettings[key];

            return settingValue ?? defaultValue;
        }

        public static string GetConnectionString(string key)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[key];

            return connectionString == null ? null : connectionString.ConnectionString;
        }


        public static string GetMasterConnectionString()
        {
            var masterConnectionStringKey = ConfigurationManager.AppSettings["MasterDatabaseConnectionString"];
            if (string.IsNullOrWhiteSpace(masterConnectionStringKey))
            {
                throw new Exception("MasterDatabaseConnectionString entry could not be found in the config file.");
            }
            var connectionString = ConfigurationManager.ConnectionStrings[masterConnectionStringKey];

            return connectionString == null ? null : connectionString.ConnectionString;
        }

        public static bool TryGetSettingAndParse<T>(string key, T defaultValue, out T result)
        {
            result = defaultValue;

            var value = ConfigurationManager.AppSettings[key];

            if (value == null)
                return false;

            if (ReflectionHelper.TryParseOrDefault<T>(value, out result, defaultValue))
                return true;

            return false;
        }
    }
}