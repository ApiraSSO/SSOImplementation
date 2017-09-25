using System;
using System.IdentityModel.Metadata;
using System.IdentityModel.Tokens;
using Kernel.Federation.MetaData.Configuration.Cryptography;
using Kernel.Federation.MetaData.Configuration.RoleDescriptors;

namespace WsFederationMetadataProvider.Metadata.DescriptorBuilders.SSODescriptorMemberBulders
{
    internal class KeysBuilder : RoleDescriptorMemberBuilder
    {
        protected override void BuildInternal(RoleDescriptor descriptor, RoleDescriptorConfiguration configuration)
        {
            if (configuration.KeyDescriptors == null)
                throw new ArgumentNullException("keyDescriptors");

            foreach (var key in configuration.KeyDescriptors)
            {
                var certConfiguration = new X509StoreCertificateConfiguration(key.CertificateContext);
                var certificate = certConfiguration.GetX509Certificate2();

                var keyDescriptor = new KeyDescriptor();
                KeyType keyType;
                if (!Enum.TryParse<KeyType>(key.Use.ToString(), out keyType))
                {
                    throw new InvalidCastException(String.Format("Parsing to type{0} failed. Value having been tried:{1}", typeof(KeyType), key.Use));
                }

                keyDescriptor.Use = keyType;

                keyDescriptor.KeyInfo = new SecurityKeyIdentifier(new X509RawDataKeyIdentifierClause(certificate));

                descriptor.Keys.Add(keyDescriptor);
            }
        }
    }
}