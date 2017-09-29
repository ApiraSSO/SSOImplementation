using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Kernel.Cache;
using Kernel.Data;
using Kernel.Data.ORM;
using Kernel.DependancyResolver;
using Kernel.Federation.MetaData.Configuration;
using Kernel.Federation.FederationPartner;
using Kernel.Reflection;
using ORMMetadataContextProvider.FederationParty;
using ORMMetadataContextProvider.Security;
using Shared.Initialisation;

namespace ORMMetadataContextProvider.Initialisation
{
    public class ORMMetadataContextProviderInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 0; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterFactory<Func<PropertyInfo, string>>(() => x => x.Name, Lifetime.Transient);
            dependencyResolver.RegisterType<CertificateValidationConfigurationProvider>(Lifetime.Transient);
            dependencyResolver.RegisterFactory<Func<NameValueCollection>>(() => () => ConfigurationManager.AppSettings, Lifetime.Transient);
            dependencyResolver.RegisterFactory<IDbCustomConfiguration>(() => this.BuildDbCustomConfiguration(), Lifetime.Transient);
            dependencyResolver.RegisterFactory<IMetadataContextBuilder>(() =>
            {
                var cacheProvider = dependencyResolver.Resolve<ICacheProvider>();
               
                var context = dependencyResolver.Resolve<IDbContext>();
                
                return new MetadataContextBuilder(context, cacheProvider);
            }, Lifetime.Transient);

            dependencyResolver.RegisterFactory<IFederationPartyContextBuilder>(() =>
            {
                var cacheProvider = dependencyResolver.Resolve<ICacheProvider>();
                
                var context = dependencyResolver.Resolve<IDbContext>();
                return new FederationPartyContextBuilder(context, cacheProvider);
            }, Lifetime.Transient);
            
            return Task.CompletedTask;
        }

        private IDbCustomConfiguration BuildDbCustomConfiguration()
        {
            var customConfiguration = new DbCustomConfiguration();
            var models = ReflectionHelper.GetAllTypes(new[] { typeof(MetadataContextBuilder).Assembly })
               .Where(t => !t.IsAbstract && !t.IsInterface && typeof(BaseModel).IsAssignableFrom(t));
            customConfiguration.ModelsFactory = () => models;

            var seeders = ReflectionHelper.GetAllTypes(new[] { typeof(MetadataContextBuilder).Assembly })
                    .Where(t => !t.IsAbstract && !t.IsInterface && typeof(ISeeder).IsAssignableFrom(t))
                    .Select(x => (ISeeder)Activator.CreateInstance(x));
            seeders
                .OrderBy(x => x.SeedingOrder)
                .Aggregate(customConfiguration, (c, next) => { c.Seeders.Add(next); return c; });
            return customConfiguration;
        }
    }
}