using System;
using System.Collections.Generic;
using System.Linq;
using Kernel.Federation.MetaData.Configuration.EntityDescriptors;

namespace Kernel.Federation.FederationPartner
{
    public class AuthnRequestConfiguration
    {
        private readonly EntityDesriptorConfiguration _entityDesriptorConfiguration;
        public AuthnRequestConfiguration(EntityDesriptorConfiguration entityDesriptorConfiguration, Uri defaultNameid, ScopingConfiguration scopingConfiguration, FederationPartyAuthnRequestConfiguration federationPartyAuthnRequestConfiguration)
        {
            if (entityDesriptorConfiguration == null)
                throw new ArgumentNullException("entityDesriptorConfiguration");
            if (defaultNameid == null)
                throw new ArgumentNullException("defaultNameid");
            if (federationPartyAuthnRequestConfiguration == null)
                throw new ArgumentNullException("federationPartyAuthnRequestConfiguration");
            if (federationPartyAuthnRequestConfiguration.RequestedAuthnContextConfiguration == null)
                throw new ArgumentNullException("requestedAuthnContextConfiguration");

            this._entityDesriptorConfiguration = entityDesriptorConfiguration;
            this.EntityId = entityDesriptorConfiguration.EntityId;
            this.RequestId = String.Format("{0}_{1}", entityDesriptorConfiguration.Id, Guid.NewGuid().ToString());
            this.AssertionConsumerServiceIndex = (ushort)entityDesriptorConfiguration.SPSSODescriptors.SelectMany(x => x.AssertionConsumerServices)
                .Single(x => x.IsDefault.GetValueOrDefault()).Index;
            this.AudienceRestriction = new List<string> { entityDesriptorConfiguration.EntityId };
            this.ForceAuthn = federationPartyAuthnRequestConfiguration.ForceAuthn;
            this.IsPassive = federationPartyAuthnRequestConfiguration.IsPassive;
            this.Version = federationPartyAuthnRequestConfiguration.Version;
            this.EncryptNameId = false;
            this.AllowCreateNameIdPolicy = false;
            this.SupportedNameIdentifierFormats = new List<Uri>();
            this.DefaultNameIdFormat = defaultNameid;
            this.RequestedAuthnContextConfiguration = federationPartyAuthnRequestConfiguration.RequestedAuthnContextConfiguration;
            this.ScopingConfiguration = scopingConfiguration;
        }

        public bool IsPassive { get; set; }
        public bool ForceAuthn { get; set; }
        public ICollection<string> AudienceRestriction { get; }
        public Uri DefaultNameIdFormat { get; }
        public bool EncryptNameId { get; }
        public bool AllowCreateNameIdPolicy { get; }
        public ushort AssertionConsumerServiceIndex { get; }
        public string RequestId { get; }
        public string EntityId { get; }
        public string Version { get; }
        public ICollection<Uri> SupportedNameIdentifierFormats { get; }
        public RequestedAuthnContextConfiguration RequestedAuthnContextConfiguration { get; }
        public ScopingConfiguration ScopingConfiguration { get; }
    }
}