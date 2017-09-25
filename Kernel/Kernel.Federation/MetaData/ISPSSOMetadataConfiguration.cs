using System.Collections.Generic;

namespace Kernel.Federation.MetaData
{
    public interface ISPSSOMetadataConfiguration : IMetadataConfiguration
    {
        bool AuthnRequestsSigned { get; set; }

        IEnumerable<IConsumerServiceContext> ConsumerServices { get; set; }
    }
}