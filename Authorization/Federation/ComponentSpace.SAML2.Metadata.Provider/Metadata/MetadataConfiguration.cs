using System;
using System.Collections.Generic;
using System.Linq;
using Kernel.Federation.MetaData;

namespace ComponentSpace.SAML2.Metadata.Provider.Metadata
{
    public class MetadataConfiguration : IMetadataConfiguration
    {
        public MetadataConfiguration()
        {
            this.Descriptors = new List<DescriptorContext>();
        }

        public string MetadatFilePathDestination { get; set; }

        public bool SignMetadata { get; set; }

        public Uri EntityId { get; set; }

        public string DescriptorId { get; set; }

        public IEnumerable<string> SupportedProtocols { get; set; }

        public IEnumerable<ICertificateContext> Keys { get; set; }

        public virtual IEnumerable<DescriptorContext> Descriptors { get; }
    }
}