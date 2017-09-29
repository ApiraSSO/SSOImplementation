using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kernel.Federation.FederationPartner;
using Shared.Federtion;

namespace Federation.Metadata.RelyingParty.Configuration
{
    internal class FederationConfigurationManager : ConfigurationManager<MetadataBase>
    {
        public FederationConfigurationManager(IFederationPartnerContextBuilder relyingPartyContextBuilder, IConfigurationRetriever<MetadataBase> configRetriever) 
            : base(relyingPartyContextBuilder, configRetriever)
        {
        }
    }
}