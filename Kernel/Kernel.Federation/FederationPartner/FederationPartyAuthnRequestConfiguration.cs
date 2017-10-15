namespace Kernel.Federation.FederationPartner
{
    public class FederationPartyAuthnRequestConfiguration
    {
        public FederationPartyAuthnRequestConfiguration(RequestedAuthnContextConfiguration requestedAuthnContextConfiguration)
        {
            this.RequestedAuthnContextConfiguration = requestedAuthnContextConfiguration;
        }

        public bool IsPassive { get; set; }
        public bool ForceAuthn { get; set; }
        public string Version { get; set; }
        public RequestedAuthnContextConfiguration RequestedAuthnContextConfiguration { get; }
    }
}