using Kernel.Initialisation;
using Nest;

namespace ElasticSearchClient.Connection
{
    internal class ConnectionManager : IConnectionManager, IAutoRegisterAsTransient
    {
        private readonly IConnectionSettingsProvider _connectionSettingsProvider;
        public ConnectionManager(IConnectionSettingsProvider connectionSettingsProvider)
        {
            this._connectionSettingsProvider = connectionSettingsProvider;
        }
        public ConnectionSettings GetConnectionSettings()
        {
            return this._connectionSettingsProvider.DefaultSettings();
        }
    }
}