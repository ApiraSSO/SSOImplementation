using System.IdentityModel.Metadata;
using Kernel.Federation.FederationPartner;
using Shared.Federtion;

namespace Federation.Metadata.FederationPartner.Configuration
{
    internal class FederationConfigurationManager : ConfigurationManager<MetadataBase>
    {
        public FederationConfigurationManager(IFederationPartyContextBuilder federationPartyContextBuilder, IConfigurationRetriever<MetadataBase> configRetriever) 
            : base(federationPartyContextBuilder, configRetriever)
        {
        }
    }
}