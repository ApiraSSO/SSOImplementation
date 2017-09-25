using System;
using System.IdentityModel.Metadata;
using Kernel.Federation.MetaData;

namespace WsFederationMetadataProvider.Metadata.DescriptorBuilders
{
    internal class IdentityProviderSingleSignOnDescriptorBuilder : DescriptorBuilderBase<IdentityProviderSingleSignOnDescriptor>
    {
        protected override IdentityProviderSingleSignOnDescriptor BuildDescriptorInternal(IMetadataConfiguration configuration)
        {
            var idpConfiguration = configuration as IIdpSSOMetadataConfiguration;

            if (idpConfiguration == null)
                throw new InvalidCastException(string.Format("Expected type: {0} but was: {1}", typeof(IdpSSOMetadataConfiguration).Name, configuration.GetType().Name));

            var descriptor = new IdentityProviderSingleSignOnDescriptor();

            descriptor.ProtocolsSupported.Add(new Uri("http://docs.oasis-open.org/wsfed/federation/200706"));

            foreach (var sso in idpConfiguration.SingleSignOnServices)
            {
                var singleSignOnService = new ProtocolEndpoint(new Uri(sso.Binding), new Uri(sso.Location));

                descriptor.SingleSignOnServices.Add(singleSignOnService);
            }

            return descriptor;
        }
    }
}