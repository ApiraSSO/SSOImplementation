using System;
using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Kernel.Federation.MetaData;
using Kernel.Federation.MetaData.Configuration;
using Shared.Initialisation;
using WsFederationMetadataProvider.Metadata;

namespace WsFederationMetadataProvider.Initialisation
{
    public class WsFederationMetadataProviderInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 1; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<SPSSOMetadataProvider>(Lifetime.Transient);
            dependencyResolver.RegisterFactory<Func<MetadataType, MetadataContext>>(() => _ =>
            {
                var builder = dependencyResolver.Resolve<IMetadataContextBuilder>();
                using (builder)
                {
                    return builder.BuildContext();
                }
            } , Lifetime.Singleton);
           
            return Task.CompletedTask;
        }
    }
}