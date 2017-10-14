using System;
using System.Collections.Generic;
using System.Linq;
using Kernel.Federation.MetaData.Configuration.EntityDescriptors;
using Kernel.Federation.Protocols;

namespace Kernel.Federation.FederationPartner
{
    public class AuthnRequestConfiguration
    {
        private readonly EntityDesriptorConfiguration _entityDesriptorConfiguration;
        public AuthnRequestConfiguration(EntityDesriptorConfiguration entityDesriptorConfiguration, Uri defaultNameid, RequestedAuthnContextConfiguration requestedAuthnContextConfiguration)
        {
            this._entityDesriptorConfiguration = entityDesriptorConfiguration;
            this.EntityId = entityDesriptorConfiguration.EntityId;
            this.RequestId = String.Format("{0}_{1}", entityDesriptorConfiguration.Id, Guid.NewGuid().ToString());
            this.AssertionConsumerServiceIndex = (ushort)entityDesriptorConfiguration.SPSSODescriptors.SelectMany(x => x.AssertionConsumerServices)
                .Single(x => x.IsDefault.GetValueOrDefault()).Index;
            this.AudienceRestriction = new List<string> { entityDesriptorConfiguration.EntityId };
            this.ForceAuthn = false;
            this.IsPassive = false;
            this.Version = "2.0";
            this.EncryptNameId = false;
            this.AllowCreateNameIdPolicy = false;
            this.SupportedNameIdentifierFormats = new List<Uri>();
            this.DefaultNameIdFormat = defaultNameid;
            this.RequestedAuthnContextConfiguration = requestedAuthnContextConfiguration;
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
    }
}