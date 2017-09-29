using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kernel.Federation.RelyingParty;
using Shared.Federtion;

namespace Federation.Metadata.RelyingParty.Configuration
{
    internal class FederationConfigurationManager : ConfigurationManager<MetadataBase>
    {
        public FederationConfigurationManager(ITenantContextBuilder relyingPartyContextBuilder, IConfigurationRetriever<MetadataBase> configRetriever) 
            : base(relyingPartyContextBuilder, configRetriever)
        {
        }
    }
}