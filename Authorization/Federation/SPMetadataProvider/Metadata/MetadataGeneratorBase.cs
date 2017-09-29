using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Federation.FederationPartner;
using Kernel.Federation.MetaData;
using Kernel.Federation.MetaData.Configuration;
using Kernel.Federation.MetaData.Configuration.EntityDescriptors;
using Kernel.Federation.MetaData.Configuration.RoleDescriptors;
using WsFederationMetadataProvider.Metadata.DescriptorBuilders;

namespace WsFederationMetadataProvider.Metadata
{
    public abstract class MetadataGeneratorBase : IMetadataGenerator
    {
        protected IFederationMetadataDispatcher _metadataDispatcher;

        protected readonly ICertificateManager _certificateManager;
        protected readonly IMetadataSerialiser<MetadataBase> _serialiser;
        protected readonly Func<MetadataGenerateRequest, FederationPartyContext> _contextFactory;
        public MetadataGeneratorBase(IFederationMetadataDispatcher metadataDispatcher, ICertificateManager certificateManager, IMetadataSerialiser<MetadataBase> serialiser, Func<MetadataGenerateRequest, FederationPartyContext> contextFactory)
        {
            this._metadataDispatcher = metadataDispatcher;
            this._certificateManager = certificateManager;
            this._serialiser = serialiser;
            this._contextFactory = contextFactory;
        }

        public async Task CreateMetadata(MetadataGenerateRequest context)
        {
            var configuration = this._contextFactory(context);
            var sb = new StringBuilder();

            using (var xmlWriter = XmlWriter.Create(sb))
            {
                await ((IMetadataGenerator)this).CreateMetadata(configuration, xmlWriter);
            }
            var metadata = new XmlDocument();
            metadata.LoadXml(sb.ToString());
            var dispatcherContext = new DispatcherContext(metadata.DocumentElement, context.Target);
            await this._metadataDispatcher.Dispatch(dispatcherContext);
        }

        Task IMetadataGenerator.CreateMetadata(FederationPartyContext federationPartyContext, XmlWriter xmlWriter)
        {
            try
            {
                if (federationPartyContext == null)
                    throw new ArgumentNullException("federationPartyContext");

                if (federationPartyContext.MetadataContext == null)
                    throw new ArgumentNullException("metadataContext");

                var configuration = federationPartyContext.MetadataContext.EntityDesriptorConfiguration;

                var descriptors = this.GetDescriptors(configuration.SPSSODescriptors);
                
                var entityDescriptor = BuildEntityDesciptor(configuration, descriptors);
                this.SignMetadata(federationPartyContext.MetadataContext, entityDescriptor);
                var sb = new StringBuilder();
                
               
                this._serialiser.Serialise(xmlWriter, entityDescriptor);
               
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void SignMetadata(MetadataContext context, MetadataBase metadata)
        {
            if (!context.SignMetadata)
                return;
            if (context.MetadataSigningContext == null)
                throw new ArgumentNullException("metadataSigningContext");

            var signMetadataKey = context.MetadataSigningContext.KeyDescriptors.Where(k => k.IsDefault)
                    .FirstOrDefault();

            if (signMetadataKey == null)
                throw new Exception("No default certificate found");

            var certificate = this._certificateManager.GetCertificateFromContext(signMetadataKey.CertificateContext);
            var signingCredentials = new SigningCredentials(new X509AsymmetricSecurityKey(certificate), context.MetadataSigningContext.SignatureAlgorithm, context.MetadataSigningContext.DigestAlgorithm, new SecurityKeyIdentifier(new X509RawDataKeyIdentifierClause(certificate)));
            metadata.SigningCredentials = signingCredentials;
        }

        protected virtual EntityDescriptor BuildEntityDesciptor(EntityDesriptorConfiguration configuration, IEnumerable<RoleDescriptor> descriptors)
        {
            var entityDescriptor = new EntityDescriptor()
            {
                EntityId = new EntityId(configuration.EntityId),
                FederationId = configuration.Id
            };

            descriptors.Aggregate(entityDescriptor, (ed, next) =>
            {
                Assignment()(entityDescriptor, next);
                return ed;
            });

            return entityDescriptor;
        }

        protected virtual Action<EntityDescriptor, RoleDescriptor> Assignment()
        {
            return (ed, rd) => ed.RoleDescriptors.Add(rd);
        }
        protected virtual IEnumerable<RoleDescriptor> GetDescriptors(IEnumerable<RoleDescriptorConfiguration> configurations)
        {
            if (configurations == null || configurations.Count() == 0)
            {
                throw new InvalidOperationException("No descriptors provided.");
            }
            var descriptors = new List<RoleDescriptor>();
            configurations.Aggregate(descriptors, (agg, next) =>
            {
                var descriptor = DescriptorBuildersHelper.ResolveAndBuild(next);
                agg.Add(descriptor);
                return agg;
            });
            return descriptors;
        }
    }
}