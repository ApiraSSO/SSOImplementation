using System;
using System.Collections.Generic;

namespace Kernel.Federation.MetaData
{
    public interface IMetadataConfiguration
    {
        string MetadatFilePathDestination { get; set; }

        bool SignMetadata { get; set; }

        Uri EntityId { get; set; }

        string DescriptorId { get; set; }

        IEnumerable<DescriptorContext> Descriptors { get; }

        IEnumerable<string> SupportedProtocols{ get; set; }

        IEnumerable<ICertificateContext> Keys { get; set; }
    }
}