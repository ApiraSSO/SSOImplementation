using System.Xml;

namespace Kernel.Federation.MetaData
{
    public class DispatcherContext
    {
        public DispatcherContext(XmlElement metadata, MetadataPublishContext metadataPublishContext)
        {
            this.Metadata = metadata;
            this.MetadataPublishContext = metadataPublishContext;
        }
        public XmlElement Metadata { get; }
        public MetadataPublishContext MetadataPublishContext { get; }
    }
}
