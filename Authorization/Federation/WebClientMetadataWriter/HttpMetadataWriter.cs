using Kernel.Federation.MetaData;
using Shared.Federtion;

namespace WebClientMetadataWriter
{
    internal class HttpMetadataWriter : MetadataWriter
    {
        protected override bool CanWrite(MetadataPublishContext target)
        {
            return (target.MetadataPublishProtocol & MetadataPublishProtocol.Http) == MetadataPublishProtocol.Http;
        }
    }
}