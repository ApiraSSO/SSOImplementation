using Nest;

namespace ElasticSearchClient.Connection.Modifiers
{
    internal class DefaultIndexModifier : ConnectionSettingModifier
    {
        protected internal override ConnectionSettings Modify(ConnectionSettings settings)
        {
            return settings.DefaultIndex("dev");
        }
    }
}