namespace Kernel.Federation.FederationPartner
{
    public class FederationPartyAuthnRequestConfiguration
    {
        public FederationPartyAuthnRequestConfiguration(RequestedAuthnContextConfiguration requestedAuthnContextConfiguration, DefaultNameId defaultNameId)
        {
            this.RequestedAuthnContextConfiguration = requestedAuthnContextConfiguration;
            this.DefaultNameId = defaultNameId;
        }

        public bool IsPassive { get; set; }
        public bool ForceAuthn { get; set; }
        public string Version { get; set; }
        public RequestedAuthnContextConfiguration RequestedAuthnContextConfiguration { get; }
        public DefaultNameId DefaultNameId { get; }
    }
}