using System.Threading.Tasks;
using System.Xml;
using Kernel.Federation.FederationPartner;

namespace Kernel.Federation.MetaData
{
    public interface IMetadataGenerator
    {
        Task CreateMetadata(FederationPartyConfiguration federationPartyContext, XmlWriter xmlWriter);
        Task CreateMetadata(MetadataGenerateRequest context);
    }
}