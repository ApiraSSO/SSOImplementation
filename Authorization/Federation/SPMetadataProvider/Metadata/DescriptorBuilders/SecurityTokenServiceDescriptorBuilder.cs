using System;
using System.IdentityModel.Metadata;
using System.IdentityModel.Protocols.WSTrust;
using Kernel.Federation.MetaData;

namespace WsFederationMetadataProvider.Metadata.DescriptorBuilders
{
    internal class SecurityTokenServiceDescriptorBuilder : DescriptorBuilderBase<SecurityTokenServiceDescriptor>
    {
        protected override SecurityTokenServiceDescriptor BuildDescriptorInternal(IMetadataConfiguration configuration)
        {
            var tokenService = new SecurityTokenServiceDescriptor();
            tokenService.ServiceDescription = "http://localhost:8080/idp/status";
            //tokenService.Keys.Add(GetSigningKeyDescriptor());

            //tokenService.PassiveRequestorEndpoints.Add(new EndpointReference(passiveRequestorEndpoint));

            //tokenService.TokenTypesOffered.Add(new Uri(TokenTypes.OasisWssSaml11TokenProfile11));
            //tokenService.TokenTypesOffered.Add(new Uri(TokenTypes.OasisWssSaml2TokenProfile11));

            //ClaimsRepository.GetSupportedClaimTypes().ToList().ForEach(claimType => tokenService.ClaimTypesOffered.Add(new DisplayClaim(claimType)));
            tokenService.ProtocolsSupported.Add(new Uri("http://docs.oasis-open.org/wsfed/federation/200706"));

            //if (ConfigurationRepository.WSTrust.Enabled && ConfigurationRepository.WSTrust.EnableMessageSecurity)
            //{
            //    var addressMessageUserName = new EndpointAddress(_endpoints.WSTrustMessageUserName, null, null, CreateMetadataReader(_endpoints.WSTrustMex), null);
            //    tokenService.SecurityTokenServiceEndpoints.Add(new EndpointReference(addressMessageUserName.Uri.AbsoluteUri));

            //    if (ConfigurationRepository.WSTrust.EnableClientCertificateAuthentication)
            //    {
            //        var addressMessageCertificate = new EndpointAddress(_endpoints.WSTrustMessageCertificate, null, null, CreateMetadataReader(_endpoints.WSTrustMex), null);
            //        tokenService.SecurityTokenServiceEndpoints.Add(new EndpointReference(addressMessageCertificate.Uri.AbsoluteUri));
            //    }
            //}
            //if (ConfigurationRepository.WSTrust.Enabled && ConfigurationRepository.WSTrust.EnableMixedModeSecurity)
            //{
            //    var addressMixedUserName = new EndpointAddress(_endpoints.WSTrustMixedUserName, null, null, CreateMetadataReader(_endpoints.WSTrustMex), null);
            tokenService.SecurityTokenServiceEndpoints.Add(new EndpointReference("http://localhost:8080/idp/status"));

            //    if (ConfigurationRepository.WSTrust.EnableClientCertificateAuthentication)
            //    {
            //        var addressMixedCertificate = new EndpointAddress(_endpoints.WSTrustMixedCertificate, null, null, CreateMetadataReader(_endpoints.WSTrustMex), null);
            //        tokenService.SecurityTokenServiceEndpoints.Add(new EndpointReference(addressMixedCertificate.Uri.AbsoluteUri));
            //    }
            //}

            //if (tokenService.SecurityTokenServiceEndpoints.Count == 0)
            //    tokenService.SecurityTokenServiceEndpoints.Add(new EndpointReference(_endpoints.WSFederation.AbsoluteUri));

            return tokenService;
        }
    }
}