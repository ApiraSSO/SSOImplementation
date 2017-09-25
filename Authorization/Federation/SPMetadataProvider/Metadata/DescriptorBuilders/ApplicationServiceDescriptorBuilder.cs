using System;
using System.IdentityModel.Metadata;
using Kernel.Federation.MetaData;
using Kernel.Federation.MetaData.Configuration.RoleDescriptors;

namespace WsFederationMetadataProvider.Metadata.DescriptorBuilders
{
    internal class ApplicationServiceDescriptorBuilder : DescriptorBuilderBase<ApplicationServiceDescriptor>
    {
        protected override ApplicationServiceDescriptor BuildDescriptorInternal(RoleDescriptorConfiguration configuration)
        {
            var idpConfiguration = configuration as IIdpSSOMetadataConfiguration;

            if (idpConfiguration == null)
                throw new InvalidCastException(string.Format("Expected type: {0} but was: {1}", typeof(IdpSSOMetadataConfiguration).Name, configuration.GetType().Name));

            var appDescriptor = new ApplicationServiceDescriptor();

            appDescriptor.ServiceDescription = "http://localhost:8080/idp/status";
            //appDescriptor.Keys.Add(GetSigningKeyDescriptor());

            //appDescriptor.PassiveRequestorEndpoints.Add(new EndpointReference("http://docs.oasis-open.org/wsfed/federation/200706"));
            //appDescriptor.TokenTypesOffered.Add(new Uri(TokenTypes.OasisWssSaml11TokenProfile11));
            //appDescriptor.TokenTypesOffered.Add(new Uri(TokenTypes.OasisWssSaml2TokenProfile11));

            //ClaimsRepository.GetSupportedClaimTypes().ToList().ForEach(claimType => appDescriptor.ClaimTypesOffered.Add(new DisplayClaim(claimType)));
            appDescriptor.ProtocolsSupported.Add(new Uri("http://docs.oasis-open.org/wsfed/federation/200706"));

            return appDescriptor;

            //descriptor.ProtocolsSupported.Add(new Uri("http://docs.oasis-open.org/wsfed/federation/200706"));

            //foreach (var sso in idpConfiguration.SingleSignOnServices)
            //{
            //    var singleSignOnService = new ProtocolEndpoint(new Uri(sso.Binding), new Uri(sso.Location));

            //    descriptor.SingleSignOnServices.Add(singleSignOnService);
            //}

            //return descriptor;
        }
    }
}