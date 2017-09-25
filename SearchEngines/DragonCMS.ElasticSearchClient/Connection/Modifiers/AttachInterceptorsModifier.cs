using Nest;

namespace ElasticSearchClient.Connection.Modifiers
{
    internal class AttachInterceptorsModifier : ConnectionSettingModifier
    {
        protected internal override ConnectionSettings Modify(ConnectionSettings settings)
        {
            return settings.OnRequestDataCreated(r => RequestInterceptor.InterceptRequestDataCreated(r))
                .OnRequestCompleted(r => RequestInterceptor.InterceptRequestComplete(r));
        }
    }
}