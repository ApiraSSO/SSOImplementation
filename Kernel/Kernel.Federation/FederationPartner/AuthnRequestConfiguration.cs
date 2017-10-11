using System;
using System.Collections.Generic;
using System.Linq;
using Kernel.Federation.MetaData.Configuration.EntityDescriptors;

namespace Kernel.Federation.FederationPartner
{
    public class AuthnRequestConfiguration
    {
        private readonly EntityDesriptorConfiguration _entityDesriptorConfiguration;
        public AuthnRequestConfiguration(EntityDesriptorConfiguration entityDesriptorConfiguration)
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
        }

        public bool IsPassive { get; set; }
        public bool ForceAuthn { get; set; }
        public ICollection<string> AudienceRestriction { get; }
        public string NamePolicy { get; }
        public ushort AssertionConsumerServiceIndex { get; }
        public string RequestId { get; }
        public string EntityId { get; }
        public string Version { get; }
    }
}