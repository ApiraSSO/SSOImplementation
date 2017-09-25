using Kernel.Initialisation;
using Nest;

namespace ElasticSearchClient.Connection.Modifiers
{
    internal abstract class ConnectionSettingModifier : IAutoRegisterAsTransient
    {
        protected internal abstract ConnectionSettings Modify(ConnectionSettings settings);
    }
}