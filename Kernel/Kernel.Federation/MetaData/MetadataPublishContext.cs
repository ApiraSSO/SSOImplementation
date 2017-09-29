using System;
using System.IO;

namespace Kernel.Federation.MetaData
{
    public class MetadataPublishContext
    {
        public MetadataPublishContext(Stream targetStream, MetadataPublishProtocol metadataPublishProtocol)
        {
            if (targetStream == null)
                throw new ArgumentNullException("targetStream");

            this.TargetStream = targetStream;
            this.MetadataPublishProtocol = metadataPublishProtocol;
        }
        public Stream TargetStream { get; }

        public MetadataPublishProtocol MetadataPublishProtocol { get; }
    }
}