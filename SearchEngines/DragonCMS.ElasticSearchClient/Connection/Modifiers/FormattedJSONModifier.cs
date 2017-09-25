using Nest;

namespace ElasticSearchClient.Connection.Modifiers
{
    internal class FormattedJSONModifier : ConnectionSettingModifier
    {
        protected internal override ConnectionSettings Modify(ConnectionSettings settings)
        {
            return settings.PrettyJson();
        }
    }
}